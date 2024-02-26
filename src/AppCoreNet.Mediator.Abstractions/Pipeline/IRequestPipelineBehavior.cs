// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a behavior for the request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request that is handled.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="context">The request context.</param>
    /// <param name="next">The behavior which should be invoked next.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task HandleAsync(
        IRequestContext<TRequest, TResponse> context,
        RequestPipelineDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken = default);
}