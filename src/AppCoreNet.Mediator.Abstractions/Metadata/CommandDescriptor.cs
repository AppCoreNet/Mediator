// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Describes a command type.
/// </summary>
public class CommandDescriptor
{
    /// <summary>
    /// Gets the type of the command.
    /// </summary>
    public Type CommandType { get; }

    /// <summary>
    /// Gets the metadata of the command type.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDescriptor"/> class.
    /// </summary>
    /// <param name="commandType">The type of the command.</param>
    /// <param name="metadata">The event type metadata.</param>
    public CommandDescriptor(Type commandType, IReadOnlyDictionary<string, object> metadata)
    {
        Ensure.Arg.NotNull(commandType);
        Ensure.Arg.OfType(commandType, typeof(ICommand<>));
        Ensure.Arg.NotNull(metadata);

        CommandType = commandType;
        Metadata = metadata;
    }
}