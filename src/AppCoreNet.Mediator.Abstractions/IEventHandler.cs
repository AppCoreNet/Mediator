// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides a handler for events.
/// </summary>
/// <typeparam name="TEvent">The type of the event that is handled.</typeparam>
public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="event">The event that should be handled.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}