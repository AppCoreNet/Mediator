// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents an event pipeline.
/// </summary>
public interface IEventPipeline
{
    /// <summary>
    /// Processes an event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task InvokeAsync(IEvent @event, CancellationToken cancellationToken = default);
}