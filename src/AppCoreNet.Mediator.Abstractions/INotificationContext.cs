// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents the context when processing notifications.
/// </summary>
public interface INotificationContext
{
    /// <summary>
    /// Gets the descriptor of the notification.
    /// </summary>
    NotificationDescriptor NotificationDescriptor { get; }

    /// <summary>
    /// Gets the notification.
    /// </summary>
    INotification Notification { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context items.
    /// </summary>
    IDictionary<object, object> Items { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context features.
    /// </summary>
    IDictionary<Type, object> Features { get; }
}

/// <summary>
/// Represents typed context when processing notifications.
/// </summary>
/// <typeparam name="TNotification">The type of the notification.</typeparam>
public interface INotificationContext<out TNotification> : INotificationContext
    where TNotification : INotification
{
    /// <summary>
    /// Gets the notification.
    /// </summary>
    new TNotification Notification { get; }
}