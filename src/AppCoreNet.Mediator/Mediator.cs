// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides the default event publisher implementation.
/// </summary>
public class Mediator : IMediator
{
    private readonly ICommandPipelineFactory _commandPipelineFactory;
    private readonly IEventPipelineFactory _eventPipelineFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="eventPipelineFactory">The event pipeline factory.</param>
    /// <param name="commandPipelineFactory">The command pipeline factory.</param>
    /// <exception cref="ArgumentNullException">Some argument is <c>null</c>.</exception>
    public Mediator(ICommandPipelineFactory commandPipelineFactory, IEventPipelineFactory eventPipelineFactory)
    {
        Ensure.Arg.NotNull(commandPipelineFactory);
        Ensure.Arg.NotNull(eventPipelineFactory);

        _commandPipelineFactory = commandPipelineFactory;
        _eventPipelineFactory = eventPipelineFactory;
    }

    /// <inheritdoc />
    public async Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(command);

        ICommandPipeline<TResult> pipeline = _commandPipelineFactory.CreatePipeline(command);
        return await pipeline.InvokeAsync(command, cancellationToken)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(@event);

        IEventPipeline pipeline = _eventPipelineFactory.CreatePipeline(@event);
        await pipeline.InvokeAsync(@event, cancellationToken)
                      .ConfigureAwait(false);
    }
}