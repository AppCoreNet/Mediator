﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a handler which is invoked after a event was successfully handled.
/// </summary>
/// <typeparam name="TEvent">The type of the event that is processed.</typeparam>
public interface IPostEventHandler<in TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Invoked after the event has being handled.
    /// </summary>
    /// <param name="context">The context of the event that was handled.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handler operation.</returns>
    Task OnHandledAsync(IEventContext<TEvent> context, CancellationToken cancellationToken);
}