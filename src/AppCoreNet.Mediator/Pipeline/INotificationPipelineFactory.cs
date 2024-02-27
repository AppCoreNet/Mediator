// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a factory for <see cref="INotificationPipeline"/> instances.
/// </summary>
public interface INotificationPipelineFactory
{
    /// <summary>
    /// Resolves an <see cref="INotificationPipeline"/> for the specified <paramref name="notification"/>.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <returns>The <see cref="INotificationPipeline"/> instance.</returns>
    INotificationPipeline CreatePipeline(INotification notification);
}