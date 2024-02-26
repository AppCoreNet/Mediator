// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a request pipeline.
/// </summary>
/// <typeparam name="TResponse">The response of the request.</typeparam>
public interface IRequestPipeline<TResponse>
{
    /// <summary>
    /// Processes a request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task<TResponse> InvokeAsync(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}