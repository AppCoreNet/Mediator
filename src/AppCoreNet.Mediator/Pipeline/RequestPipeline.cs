// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class RequestPipeline<TRequest, TResponse> : IRequestPipeline<TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestDescriptorFactory _descriptorFactory;
    private readonly IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> _behaviors;
    private readonly IRequestHandler<TRequest, TResponse> _handler;
    private readonly ILogger<RequestPipeline<TRequest, TResponse>> _logger;
    private readonly IRequestContextAccessor? _contextAccessor;

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

        await _behaviors
              .Reverse()
              .Aggregate(
                  (RequestPipelineDelegate<TRequest, TResponse>)(async (c, ct) =>
                  {
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
                      ct.ThrowIfCancellationRequested();

                      await behavior.HandleAsync(c, next, ct)
                                    .ConfigureAwait(false);
                  })(
                  context,
                  cancellationToken);

        if (context.IsFailed)
        {
            exceptionDispatchInfo?.Throw();
            throw context.Error!;
        }

        return context.Response!;
    }
}