// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides access to the currently processed <see cref="ICommandContext"/>.
/// </summary>
public interface ICommandContextAccessor
{
    /// <summary>
    /// Gets or sets the current <see cref="ICommandContext"/>.
    /// </summary>
    ICommandContext? CommandContext { get; set; }
}