﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPreRequestHandler{TRequest,TResponse}"/>s when the request
/// is about to be handled.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class PreRequestHandlerBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IPreRequestHandler<TRequest, TResponse>> _handlers;
    private readonly ILogger<PreRequestHandlerBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreRequestHandlerBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPreRequestHandler{TRequest,TResponse}"/>s.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PreRequestHandlerBehavior(
        IEnumerable<IPreRequestHandler<TRequest, TResponse>> handlers,
        ILogger<PreRequestHandlerBehavior<TRequest, TResponse>> logger)
    {
        Ensure.Arg.NotNull(handlers);
        Ensure.Arg.NotNull(logger);

        _handlers = handlers;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        IRequestContext<TRequest, TResponse> context,
        RequestPipelineDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken = default)
    {
        if (!context.IsCompleted)
        {
            foreach (IPreRequestHandler<TRequest, TResponse> handler in _handlers)
            {
                _logger.InvokingPreRequestHandler(typeof(TRequest), handler.GetType());

                await handler.OnHandlingAsync(context, cancellationToken)
                             .ConfigureAwait(false);
            }
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);
    }
}