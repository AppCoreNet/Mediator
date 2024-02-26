// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class EventPipeline<TEvent> : IEventPipeline
    where TEvent : IEvent
{
    private readonly IEventDescriptorFactory _descriptorFactory;
    private readonly List<IEventPipelineBehavior<TEvent>> _behaviors;
    private readonly List<IEventHandler<TEvent>> _handlers;
    private readonly ILogger<EventPipeline<TEvent>> _logger;
    private readonly IEventContextAccessor? _contextAccessor;

    public EventPipeline(
        IEventDescriptorFactory descriptorFactory,
        IEnumerable<IEventPipelineBehavior<TEvent>> behaviors,
        IEnumerable<IEventHandler<TEvent>> handlers,
        ILogger<EventPipeline<TEvent>> logger,
        IEventContextAccessor? contextAccessor = null)
    {
        Ensure.Arg.NotNull(descriptorFactory);
        Ensure.Arg.NotNull(behaviors);
        Ensure.Arg.NotNull(handlers);
        Ensure.Arg.NotNull(logger);

        _behaviors = behaviors.ToList();
        _handlers = handlers.ToList();
        _descriptorFactory = descriptorFactory;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(@event);

        EventDescriptor descriptor = _descriptorFactory.CreateDescriptor(typeof(TEvent));
        var context = new EventContext<TEvent>(descriptor, (TEvent)@event);

        if (_contextAccessor != null)
            _contextAccessor.EventContext = context;

        try
        {
            await InvokeAsync(context, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (_contextAccessor != null)
                _contextAccessor.EventContext = null;
        }
    }

    private async Task InvokeAsync(IEventContext<TEvent> context, CancellationToken cancellationToken)
    {
        bool handlerInvoked = false;
        async Task Handler(IEventContext<TEvent> c, CancellationToken ct)
        {
            handlerInvoked = true;
            foreach (IEventHandler<TEvent> handler in _handlers)
            {
                await handler.HandleAsync(c.Event, ct);
                ct.ThrowIfCancellationRequested();
            }
        }

        _logger.PipelineProcessing(typeof(TEvent));

        var stopwatch = Stopwatch.StartNew();

        try
        {
            IEventPipelineBehavior<TEvent>? current = null;
            await ((IEnumerable<IEventPipelineBehavior<TEvent>>)_behaviors)
                  .Reverse()
                  .Aggregate(
                      (EventPipelineDelegate<TEvent>)Handler,
                      (next, behavior) => async (e, ct) =>
                      {
                          _logger.InvokingBehavior(typeof(TEvent), behavior.GetType());
                          current = behavior;
                          await behavior.HandleAsync(e, next, ct);
                      })(context, cancellationToken)
                  .ConfigureAwait(false);

            if (handlerInvoked)
            {
                _logger.PipelineProcessed(typeof(TEvent), stopwatch.Elapsed);
            }
            else
            {
                _logger.PipelineShortCircuited(typeof(TEvent), current!.GetType(), stopwatch.Elapsed);
            }
        }
        catch (Exception error)
        {
            _logger.PipelineFailed(typeof(TEvent), stopwatch.Elapsed, error);
            throw;
        }
    }
}