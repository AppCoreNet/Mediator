﻿// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCore.EventModel.Metadata;
using AppCoreNet;
using AppCoreNet.Diagnostics;

namespace AppCore.EventModel.Pipeline;

using EventContextFactoryDelegate = Func<EventDescriptor, IEvent, IEventContext>;

/// <inheritdoc />
public class EventContextFactory : IEventContextFactory
{
    private static readonly Type _eventContextType = typeof(EventContext<>);

    private static readonly ConcurrentDictionary<Type, EventContextFactoryDelegate> _eventContextFactories =
        new ConcurrentDictionary<Type, EventContextFactoryDelegate>();

    private static EventContextFactoryDelegate GetEventContextFactory(Type eventType)
    {
        return _eventContextFactories.GetOrAdd(
            eventType,
            t =>
            {
                Type eventContextType = _eventContextType.MakeGenericType(t);

                return TypeActivator.GetFactoryDelegate<EventContextFactoryDelegate>(
                    eventContextType,
                    typeof(EventDescriptor),
                    eventType);
            });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventContextFactory"/> class.
    /// </summary>
    public EventContextFactory()
    {
    }

    /// <inheritdoc />
    public IEventContext CreateContext(EventDescriptor descriptor, IEvent @event)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.NotNull(@event);

        Type eventType = @event.GetType();
        EventContextFactoryDelegate factory = GetEventContextFactory(eventType);
        return factory(descriptor, @event);
    }
}