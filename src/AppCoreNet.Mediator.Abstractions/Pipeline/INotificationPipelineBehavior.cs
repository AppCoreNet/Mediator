// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a behavior for the notification pipeline.
/// </summary>
/// <typeparam name="TNotification">The type of the notification that is notified.</typeparam>
public interface INotificationPipelineBehavior<TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Handles the specified notification.
    /// </summary>
    /// <param name="context">The notification context.</param>
    /// <param name="next">The behavior which should be invoked next.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous request operation.</returns>
    Task HandleAsync(
        INotificationContext<TNotification> context,
        NotificationPipelineDelegate<TNotification> next,
        CancellationToken cancellationToken = default);
}