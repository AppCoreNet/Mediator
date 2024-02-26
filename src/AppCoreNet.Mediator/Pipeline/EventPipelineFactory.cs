// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class EventPipelineFactory : IEventPipelineFactory
{
    private static readonly Type _eventPipelineType = typeof(EventPipeline<>);
    private static readonly ConcurrentDictionary<Type, Type> _pipelineTypes = new ();

    private readonly IActivator _activator;

    public EventPipelineFactory(IActivator activator)
    {
        Ensure.Arg.NotNull(activator);
        _activator = activator;
    }

    /// <inheritdoc />
    public IEventPipeline CreatePipeline(IEvent @event)
    {
        Ensure.Arg.NotNull(@event);
        Type pipelineType = _pipelineTypes.GetOrAdd(@event.GetType(), key => _eventPipelineType.MakeGenericType(key));
        return (IEventPipeline)_activator.CreateInstance(pipelineType) !;
    }
}