// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Feature for cancelable notifications.
/// </summary>
public interface ICancelableNotificationFeature
{
    /// <summary>
    /// Cancels the notification.
    /// </summary>
    void Cancel();
}