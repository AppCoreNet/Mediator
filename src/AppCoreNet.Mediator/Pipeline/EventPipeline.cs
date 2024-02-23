// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides the default implementation of <see cref="IEventPipeline{TEvent}"/>.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public class EventPipeline<TEvent> : IEventPipeline<TEvent>
    where TEvent : IEvent
{
    private readonly List<IEventPipelineBehavior<TEvent>> _behaviors;
    private readonly List<IEventHandler<TEvent>> _handlers;
    private readonly ILogger<EventPipeline<TEvent>> _logger;
    private readonly IEventContextAccessor? _contextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventPipeline{TEvent}"/> class.
    /// </summary>
    /// <param name="behaviors">The pipeline behaviors.</param>
    /// <param name="handlers">The event handlers.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="contextAccessor">The accessor for the current <see cref="IEventContext"/>.</param>
    public EventPipeline(
        IEnumerable<IEventPipelineBehavior<TEvent>> behaviors,
        IEnumerable<IEventHandler<TEvent>> handlers,
        ILogger<EventPipeline<TEvent>> logger,
        IEventContextAccessor? contextAccessor = null)
    {
        Ensure.Arg.NotNull(behaviors);
        Ensure.Arg.NotNull(handlers);
        Ensure.Arg.NotNull(logger);

        _behaviors = behaviors.ToList();
        _handlers = handlers.ToList();
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(IEventContext<TEvent> eventContext, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(eventContext);

        bool handlerInvoked = false;
        async Task Handler(IEventContext<TEvent> c, CancellationToken ct)
        {
            handlerInvoked = true;
            foreach (IEventHandler<TEvent> handler in _handlers)
            {
                await handler.HandleAsync(c, ct);
                ct.ThrowIfCancellationRequested();
            }
        }

        _logger.PipelineProcessing(typeof(TEvent));

        var stopwatch = Stopwatch.StartNew();

        if (_contextAccessor != null)
            _contextAccessor.EventContext = eventContext;

        try
        {
            IEventPipelineBehavior<TEvent>? current = null;
            await ((IEnumerable<IEventPipelineBehavior<TEvent>>)_behaviors)
                  .Reverse()
                  .Aggregate(
                      (EventPipelineDelegate<TEvent>) Handler,
                      (next, behavior) => async (e, ct) =>
                      {
                          _logger.InvokingBehavior(typeof(TEvent), behavior.GetType());
                          current = behavior;
                          await behavior.HandleAsync(e, next, ct);
                      }
                  )(eventContext, cancellationToken)
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
        finally
        {
            if (_contextAccessor != null)
                _contextAccessor.EventContext = null;
        }
    }

    Task IEventPipeline.ProcessAsync(IEventContext context, CancellationToken cancellationToken)
    {
        return ProcessAsync((IEventContext<TEvent>) context, cancellationToken);
    }
}