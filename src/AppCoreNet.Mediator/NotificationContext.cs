// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Default implementation of the <see cref="INotificationContext{TNotification}"/> interface.
/// </summary>
/// <typeparam name="TNotification">The type of the notification.</typeparam>
public class NotificationContext<TNotification> : INotificationContext<TNotification>
    where TNotification : INotification
{
    /// <inheritdoc />
    public NotificationDescriptor NotificationDescriptor { get; }

    /// <inheritdoc />
    public TNotification Notification { get; }

    /// <inheritdoc />
    INotification INotificationContext.Notification => Notification;

    /// <inheritdoc />
    public IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

    /// <inheritdoc />
    public IDictionary<Type, object> Features { get; } = new Dictionary<Type, object>();

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationContext{TNotification}"/> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="NotificationDescriptor"/>.</param>
    /// <param name="notification">The notification that is being processed.</param>
    public NotificationContext(NotificationDescriptor descriptor, TNotification notification)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.OfType<TNotification>(descriptor.NotificationType);
        Ensure.Arg.NotNull(notification);

        NotificationDescriptor = descriptor;
        Notification = notification;
    }
}