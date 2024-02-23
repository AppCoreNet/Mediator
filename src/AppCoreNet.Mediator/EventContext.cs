﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Default implementation of the <see cref="IEventContext{TEvent}"/> interface.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public class EventContext<TEvent> : IEventContext<TEvent>
    where TEvent : IEvent
{
    /// <inheritdoc />
    public EventDescriptor EventDescriptor { get; }

    /// <inheritdoc />
    public TEvent Event { get; }

    /// <inheritdoc />
    IEvent IEventContext.Event => Event;

    /// <inheritdoc />
    public IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

    /// <inheritdoc />
    public IDictionary<Type, object> Features { get; } = new Dictionary<Type, object>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventContext{TEvent}"/> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="EventDescriptor"/>.</param>
    /// <param name="event">The event that is being processed.</param>
    public EventContext(EventDescriptor descriptor, TEvent @event)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.OfType<TEvent>(descriptor.EventType);
        Ensure.Arg.NotNull(@event);

        EventDescriptor = descriptor;
        Event = @event;
    }
}