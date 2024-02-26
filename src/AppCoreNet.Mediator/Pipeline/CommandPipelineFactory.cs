// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class CommandPipelineFactory : ICommandPipelineFactory
{
    private readonly IActivator _activator;
    private static readonly Type _commandPipelineType = typeof(CommandPipeline<,>);
    private static readonly ConcurrentDictionary<Type, Type> _commandPipelineTypes = new ();

    private static Type GetCommandPipelineType(Type commandType)
    {
        return _commandPipelineTypes.GetOrAdd(commandType, t =>
        {
            Type commandInterfaceType = t.GetClosedTypeOf(typeof(ICommand<>));
            return _commandPipelineType.MakeGenericType(t, commandInterfaceType.GenericTypeArguments[0]);
        });
    }

    public CommandPipelineFactory(IActivator activator)
    {
        _activator = activator;
    }

    /// <inheritdoc/>
    public ICommandPipeline<TResult> CreatePipeline<TResult>(ICommand<TResult> command)
    {
        Ensure.Arg.NotNull(command);
        Type commandPipelineType = GetCommandPipelineType(command.GetType());
        return (ICommandPipeline<TResult>)_activator.CreateInstance(commandPipelineType) !;
    }
}