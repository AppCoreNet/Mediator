// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides access to the currently processed <see cref="INotificationContext"/>.
/// </summary>
public interface INotificationContextAccessor
{
    /// <summary>
    /// Gets or sets the current <see cref="INotificationContext"/>.
    /// </summary>
    INotificationContext? CurrentContext { get; set; }
}