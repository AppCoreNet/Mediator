// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class RequestPipeline<TRequest, TResponse> : IRequestPipeline<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestDescriptorFactory _descriptorFactory;
    private readonly IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> _behaviors;
    private readonly IRequestHandler<TRequest, TResponse> _handler;
    private readonly ILogger<RequestPipeline<TRequest, TResponse>> _logger;
    private readonly IRequestContextAccessor? _contextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPipeline{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="descriptorFactory">The <see cref="IRequestDescriptorFactory"/>.</param>
    /// <param name="behaviors">The request pipeline behaviors.</param>
    /// <param name="handler">The request handler.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    /// <param name="contextAccessor">The optional <see cref="IRequestContextAccessor"/>.</param>
    public RequestPipeline(
        IRequestDescriptorFactory descriptorFactory,
        IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> behaviors,
        IRequestHandler<TRequest, TResponse> handler,
        ILogger<RequestPipeline<TRequest, TResponse>> logger,
        IRequestContextAccessor? contextAccessor = null)
    {
        _descriptorFactory = descriptorFactory;
        _behaviors = behaviors;
        _handler = handler;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public async Task<TResponse> InvokeAsync(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        RequestDescriptor descriptor = _descriptorFactory.CreateDescriptor(typeof(TRequest));
        var context = new RequestContext<TRequest, TResponse>(descriptor, (TRequest)request);

        if (_contextAccessor != null)
            _contextAccessor.CurrentContext = context;

        try
        {
            return await InvokeAsync(context, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (_contextAccessor != null)
                _contextAccessor.CurrentContext = null;
        }
    }

    private async Task<TResponse> InvokeAsync(IRequestContext<TRequest, TResponse> context, CancellationToken cancellationToken)
    {
        ExceptionDispatchInfo? exceptionDispatchInfo = null;

        _logger.RequestProcessing(typeof(TRequest));

        var stopwatch = Stopwatch.StartNew();

        bool handlerInvoked = false;
        IRequestPipelineBehavior<TRequest, TResponse>? current = null;

        try
        {
            await _behaviors
                  .Reverse()
                  .Aggregate(
                      (RequestPipelineDelegate<TRequest, TResponse>)(async (c, ct) =>
                      {
                          handlerInvoked = true;
                          if (!c.IsCompleted)
                          {
                              ct.ThrowIfCancellationRequested();
                              try
                              {
                                  TResponse result = await _handler.HandleAsync(c.Request, ct)
                                                                   .ConfigureAwait(false);

                                  c.Complete(result);
                              }
                              catch (Exception error)
                              {
                                  exceptionDispatchInfo = ExceptionDispatchInfo.Capture(error);
                                  c.Fail(error);
                              }
                          }
                      }),
                      (next, behavior) => async (c, ct) =>
                      {
                          _logger.InvokingRequestBehavior(typeof(TRequest), behavior.GetType());

                          current = behavior;
                          await behavior.HandleAsync(c, next, ct)
                                        .ConfigureAwait(false);

                          ct.ThrowIfCancellationRequested();
                      })(
                      context,
                      cancellationToken);

            if (context.IsFailed)
            {
                exceptionDispatchInfo?.Throw();
                throw context.Error!;
            }
        }
        catch (Exception error)
        {
            _logger.RequestFailed(typeof(TRequest), stopwatch.Elapsed, error);
            throw;
        }

        if (handlerInvoked)
        {
            _logger.RequestProcessed(typeof(TRequest), stopwatch.Elapsed);
        }
        else
        {
            _logger.RequestShortCircuited(typeof(TRequest), current!.GetType(), stopwatch.Elapsed);
        }

        return context.Response!;
    }
}