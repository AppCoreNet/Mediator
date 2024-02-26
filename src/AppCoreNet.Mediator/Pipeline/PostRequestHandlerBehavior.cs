// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPostRequestHandler{TRequest,TResponse}"/>s when the
/// command has been successfully handled.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class PostRequestHandlerBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IPostRequestHandler<TRequest, TResponse>> _handlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostRequestHandlerBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPostRequestHandler{TRequest,TResponse}"/>s.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PostRequestHandlerBehavior(IEnumerable<IPostRequestHandler<TRequest, TResponse>> handlers)
    {
        Ensure.Arg.NotNull(handlers);
        _handlers = handlers;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        IRequestContext<TRequest, TResponse> context,
        RequestPipelineDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken = default)
    {
        await next(context, cancellationToken)
            .ConfigureAwait(false);

        if (!context.IsFailed)
        {
            foreach (IPostRequestHandler<TRequest, TResponse> handler in _handlers)
            {
                await handler.OnHandledAsync(context, cancellationToken)
                             .ConfigureAwait(false);
            }
        }
    }
}