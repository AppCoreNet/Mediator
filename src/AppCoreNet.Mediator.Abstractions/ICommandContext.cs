// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents the context when processing a command.
/// </summary>
public interface ICommandContext
{
    /// <summary>
    /// Gets the <see cref="CommandDescriptor"/> of the event.
    /// </summary>
    CommandDescriptor CommandDescriptor { get; }

    /// <summary>
    /// Gets the command.
    /// </summary>
    ICommand<object> Command { get; }

    /// <summary>
    /// Gets the command result.
    /// </summary>
    object? Result { get; }

    /// <summary>
    /// Gets error that occured.
    /// </summary>
    Exception? Error { get; }

    /// <summary>
    /// Gets a value indicating whether the command has been completed (successfully or not).
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Gets a value indicating whether the command has failed.
    /// </summary>
    bool IsFailed { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context items.
    /// </summary>
    IDictionary<object, object> Items { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context features.
    /// </summary>
    IDictionary<Type, object> Features { get; }

    /// <summary>
    /// Completes the command with the specified result.
    /// </summary>
    /// <param name="result">The command result.</param>
    void Complete(object result);

    /// <summary>
    /// Completes the command with the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="error">The <see cref="Exception"/> which occured during processing.</param>
    void Fail(Exception error);
}

/// <summary>
/// Represents typed context when processing a command.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommandContext<out TCommand, TResult> : ICommandContext
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Gets the command.
    /// </summary>
    new TCommand Command { get; }

    /// <summary>
    /// Gets the command result.
    /// </summary>
    new TResult? Result { get; }

    /// <summary>
    /// Completes the command with the specified result.
    /// </summary>
    /// <param name="result">The command result.</param>
    void Complete(TResult result);
}