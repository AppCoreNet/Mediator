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
    /// <param name="command">The request to process.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an event.
    /// </summary>
    /// <param name="event">The notification to publish.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
}