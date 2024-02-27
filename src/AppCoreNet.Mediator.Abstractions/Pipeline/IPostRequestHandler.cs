// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a handler which is invoked after a request was successfully processed.
/// </summary>
/// <typeparam name="TRequest">The type of the request that is handled.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IPostRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Invoked after the request has been processed.
    /// </summary>
    /// <param name="context">The context of the request that was processed.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous request operation.</returns>
    Task OnHandledAsync(
        IRequestContext<TRequest, TResponse> context,
        CancellationToken cancellationToken = default);
}