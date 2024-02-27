// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents the mediator.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Processes a request.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a notification.
    /// </summary>
    /// <param name="notification">The notification to publish.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous notification operation.</returns>
    Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default);
}