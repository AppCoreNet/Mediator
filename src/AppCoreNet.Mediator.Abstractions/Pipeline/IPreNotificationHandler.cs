// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a handler which is invoked before a notification is handled.
/// </summary>
/// <typeparam name="TNotification">The type of the notification that is processed.</typeparam>
public interface IPreNotificationHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Invoked before the notification is being handled.
    /// </summary>
    /// <param name="context">The context of the notification that is about to be handled.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous request operation.</returns>
    Task OnHandlingAsync(INotificationContext<TNotification> context, CancellationToken cancellationToken = default);
}