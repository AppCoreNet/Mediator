// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Threading;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Default implementation of the <see cref="ICommandContextAccessor"/> interface.
/// </summary>
public class CommandContextAccessor : ICommandContextAccessor
{
    private readonly AsyncLocal<ICommandContext?> _commandContext = new AsyncLocal<ICommandContext?>();

    /// <inheritdoc />
    public ICommandContext? CommandContext
    {
        get => _commandContext.Value;
        set => _commandContext.Value = value;
    }
}