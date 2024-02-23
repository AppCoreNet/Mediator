// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Mediator.Metadata;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides the default command processor implementation.
/// </summary>
public class CommandProcessor : ICommandProcessor
{
    private readonly IActivator _activator;
    private readonly ICommandDescriptorFactory _commandDescriptorFactory;
    private readonly ICommandContextAccessor? _commandContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
    /// </summary>
    /// <param name="activator">The <see cref="IActivator"/> used to resolve handlers and behaviors.</param>
    /// <param name="commandDescriptorFactory">The factory for <see cref="CommandDescriptor"/>'s.</param>
    /// <param name="commandContextAccessor">The accessor for the current <see cref="ICommandContext"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="activator"/> is <c>null</c>.</exception>
    public CommandProcessor(
        IActivator activator,
        ICommandDescriptorFactory commandDescriptorFactory,
        ICommandContextAccessor? commandContextAccessor = null)
    {
        Ensure.Arg.NotNull(commandDescriptorFactory);
        Ensure.Arg.NotNull(activator);

        _commandDescriptorFactory = commandDescriptorFactory;
        _commandContextAccessor = commandContextAccessor;
        _activator = activator;
    }

    /// <inheritdoc />
    public async Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(command);

        Type commandType = command.GetType();

        var pipeline =
            (ICommandPipeline<TResult>) CommandPipelineFactory.CreateCommandPipeline(commandType, _activator);

        CommandDescriptor commandDescriptor = _commandDescriptorFactory.CreateDescriptor(commandType);
        ICommandContext commandContext = pipeline.CreateCommandContext(commandDescriptor, command);

        if (_commandContextAccessor != null)
            _commandContextAccessor.CommandContext = commandContext;

        try
        {
            return await pipeline.InvokeAsync(commandContext, cancellationToken)
                                 .ConfigureAwait(false);
        }
        finally
        {
            if (_commandContextAccessor != null)
                _commandContextAccessor.CommandContext = null;
        }
    }
}