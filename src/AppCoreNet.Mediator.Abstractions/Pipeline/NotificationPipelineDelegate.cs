// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a delegate in the notification pipeline.
/// </summary>
/// <typeparam name="TNotification">The type of the notification.</typeparam>
/// <param name="context">The notification context.</param>
/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous notification operation.</returns>
public delegate Task NotificationPipelineDelegate<in TNotification>(
    INotificationContext<TNotification> context,
    CancellationToken cancellationToken)
    where TNotification : INotification;