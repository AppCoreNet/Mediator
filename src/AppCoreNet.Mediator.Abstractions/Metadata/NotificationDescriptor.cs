// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Describes a notification type.
/// </summary>
public class NotificationDescriptor
{
    /// <summary>
    /// Gets the type of the notification.
    /// </summary>
    public Type NotificationType { get; }

    /// <summary>
    /// Gets the metadata of the notification type.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationDescriptor"/> class.
    /// </summary>
    /// <param name="notificationType">The type of the notification.</param>
    /// <param name="metadata">The notification type metadata.</param>
    public NotificationDescriptor(Type notificationType, IReadOnlyDictionary<string, object> metadata)
    {
        Ensure.Arg.NotNull(notificationType);
        Ensure.Arg.OfType<INotification>(notificationType);
        Ensure.Arg.NotNull(metadata);

        NotificationType = notificationType;
        Metadata = metadata;
    }
}