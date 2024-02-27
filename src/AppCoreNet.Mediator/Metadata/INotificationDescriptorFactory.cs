// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which instantiates <see cref="NotificationDescriptor"/> objects.
/// </summary>
public interface INotificationDescriptorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="NotificationDescriptor"/> for the specified <paramref name="notificationType"/>.
    /// </summary>
    /// <param name="notificationType">The type of the notification.</param>
    /// <returns>The <see cref="NotificationDescriptor"/>.</returns>
    NotificationDescriptor CreateDescriptor(Type notificationType);
}