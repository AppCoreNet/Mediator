// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides access to the currently processed <see cref="IEventContext"/>.
/// </summary>
public interface IEventContextAccessor
{
    /// <summary>
    /// Gets or sets the current <see cref="IEventContext"/>.
    /// </summary>
    IEventContext? EventContext { get; set; }
}