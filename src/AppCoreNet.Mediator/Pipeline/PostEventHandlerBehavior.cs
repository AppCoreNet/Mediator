// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppCore.EventModel.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPostEventHandler{TEvent}"/>s.
/// </summary>
/// <typeparam name="TEvent">The type of the event that is handled.</typeparam>
public class PostEventHandlerBehavior<TEvent> : IEventPipelineBehavior<TEvent>
    where TEvent : IEvent
{
    private readonly List<IPostEventHandler<TEvent>> _handlers;
    private readonly ILogger<PostEventHandlerBehavior<TEvent>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostEventHandlerBehavior{TEvent}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPostEventHandler{TEvent}"/>s.</param>
    /// <param name="logger">The <see cref="ILogger{TCategory}"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PostEventHandlerBehavior(IEnumerable<IPostEventHandler<TEvent>> handlers, ILogger<PostEventHandlerBehavior<TEvent>> logger)
    {
        Ensure.Arg.NotNull(handlers);
        Ensure.Arg.NotNull(logger);

        _handlers = handlers.ToList();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        IEventContext<TEvent> context,
        EventPipelineDelegate<TEvent> next,
        CancellationToken cancellationToken)
    {
        await next(context, cancellationToken)
            .ConfigureAwait(false);

        foreach (IPostEventHandler<TEvent> handler in _handlers)
        {
            _logger.InvokingPostEventHandler(typeof(TEvent), handler.GetType());

            await handler.OnHandledAsync(context, cancellationToken)
                         .ConfigureAwait(false);
        }
    }
}