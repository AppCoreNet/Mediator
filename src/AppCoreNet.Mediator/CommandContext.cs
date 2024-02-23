// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.CommandModel.Metadata;
using AppCoreNet.Diagnostics;

namespace AppCore.CommandModel;

/// <summary>
/// Default implementation of the <see cref="ICommandContext{TCommand,TResult}"/> interface.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class CommandContext<TCommand, TResult> : ICommandContext<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <inheritdoc />
    public CommandDescriptor CommandDescriptor { get; }

    /// <inheritdoc />
    public TCommand Command { get; }

    /// <inheritdoc />
    public TResult? Result { get; private set; }

    /// <inheritdoc />
    public Exception? Error { get; private set; }

    /// <inheritdoc/>
    object? ICommandContext.Result => Result;

    /// <inheritdoc />
    public bool IsCompleted { get; private set; }

    /// <inheritdoc />
    public bool IsFailed => Error != null;

    /// <inheritdoc />
    ICommand<object> ICommandContext.Command => (ICommand<object>)Command;

    /// <inheritdoc />
    public IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

    /// <inheritdoc />
    public IDictionary<Type, object> Features { get; } = new Dictionary<Type, object>();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext{TCommand,TResult}"/> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="CommandDescriptor"/>.</param>
    /// <param name="command">The command that is being processed.</param>
    public CommandContext(CommandDescriptor descriptor, TCommand command)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.OfType<TCommand>(descriptor.CommandType, nameof(descriptor));
        Ensure.Arg.NotNull(command);

        CommandDescriptor = descriptor;
        Command = command;
    }

    void ICommandContext.Complete(object result)
    {
        Complete((TResult)result);
    }

    /// <inheritdoc />
    public void Fail(Exception error)
    {
        Ensure.Arg.NotNull(error);
        IsCompleted = true;
        Error = error;
    }

    /// <inheritdoc />
    public void Complete(TResult result)
    {
        Ensure.Arg.NotNull(result);
        IsCompleted = true;
        Result = result;
    }
}