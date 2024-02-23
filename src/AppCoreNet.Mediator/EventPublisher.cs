// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides the default event publisher implementation.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IEventDescriptorFactory _descriptorFactory;
    private readonly IEventContextFactory _contextFactory;
    private readonly IEventPipelineResolver _pipelineResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventPublisher"/> class.
    /// </summary>
    /// <param name="descriptorFactory">The factory for <see cref="EventDescriptor"/>.</param>
    /// <param name="contextFactory">The factory for <see cref="IEventContext"/>'s.</param>
    /// <param name="pipelineResolver">The event pipeline resolver.</param>
    /// <exception cref="ArgumentNullException">Some argument is <c>null</c>.</exception>
    public EventPublisher(
        IEventDescriptorFactory descriptorFactory,
        IEventContextFactory contextFactory,
        IEventPipelineResolver pipelineResolver)
    {
        Ensure.Arg.NotNull(descriptorFactory);
        Ensure.Arg.NotNull(contextFactory);
        Ensure.Arg.NotNull(pipelineResolver);

        _descriptorFactory = descriptorFactory;
        _contextFactory = contextFactory;
        _pipelineResolver = pipelineResolver;
    }

    /// <inheritdoc />
    public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(@event);

        Type eventType = @event.GetType();

        EventDescriptor eventDescriptor = _descriptorFactory.CreateDescriptor(eventType);
        IEventContext eventContext = _contextFactory.CreateContext(eventDescriptor, @event);
        IEventPipeline pipeline = _pipelineResolver.Resolve(eventType);
        await pipeline.ProcessAsync(eventContext, cancellationToken)
                      .ConfigureAwait(false);
    }
}