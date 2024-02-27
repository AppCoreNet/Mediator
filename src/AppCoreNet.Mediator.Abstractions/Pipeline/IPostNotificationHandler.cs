// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a handler which is invoked after a notification was successfully handled.
/// </summary>
/// <typeparam name="TNotification">The type of the notification that is processed.</typeparam>
public interface IPostNotificationHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Invoked after the notification has being handled.
    /// </summary>
    /// <param name="context">The context of the notification that was handled.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handler operation.</returns>
    Task OnHandledAsync(INotificationContext<TNotification> context, CancellationToken cancellationToken = default);
}