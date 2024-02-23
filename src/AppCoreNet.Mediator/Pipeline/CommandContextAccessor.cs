// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;

namespace AppCoreNet.Mediator.Pipeline;

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