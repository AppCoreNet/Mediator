// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents an notification pipeline.
/// </summary>
public interface INotificationPipeline
{
    /// <summary>
    /// Processes an notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous notification operation.</returns>
    Task InvokeAsync(INotification notification, CancellationToken cancellationToken = default);
}