// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class NotificationPipelineFactory : INotificationPipelineFactory
{
    private static readonly Type _pipelineType = typeof(NotificationPipeline<>);
    private static readonly ConcurrentDictionary<Type, Type> _pipelineTypes = new ();

    private readonly IActivator _activator;

    public NotificationPipelineFactory(IActivator activator)
    {
        Ensure.Arg.NotNull(activator);
        _activator = activator;
    }

    /// <inheritdoc />
    public INotificationPipeline CreatePipeline(INotification notification)
    {
        Ensure.Arg.NotNull(notification);
        Type pipelineType = _pipelineTypes.GetOrAdd(notification.GetType(), key => _pipelineType.MakeGenericType(key));
        return (INotificationPipeline)_activator.CreateInstance(pipelineType) !;
    }
}