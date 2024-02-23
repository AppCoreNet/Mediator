// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.CommandModel.Pipeline;

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