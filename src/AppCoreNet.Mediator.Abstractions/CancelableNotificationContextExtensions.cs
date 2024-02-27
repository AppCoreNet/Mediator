// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="INotificationContext"/> type.
/// </summary>
public static class CancelableNotificationContextExtensions
{
    /// <summary>
    /// Gets a value indicating whether the notification is cancelable.
    /// </summary>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    /// <returns><c>true</c> if the notification can be canceled; <c>false</c> otherwise.</returns>
    public static bool IsCancelable(this INotificationContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.HasFeature<ICancelableNotificationFeature>();
    }

    /// <summary>
    /// Cancels the notification.
    /// </summary>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    public static void Cancel(this INotificationContext context)
    {
        Ensure.Arg.NotNull(context);
        var feature = context.GetFeature<ICancelableNotificationFeature>();
        feature.Cancel();
    }
}