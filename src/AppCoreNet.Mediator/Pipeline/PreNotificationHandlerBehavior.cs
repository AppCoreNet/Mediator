// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPreNotificationHandler{TNotification}"/>s.
/// </summary>
/// <typeparam name="TNotification">The type of the notification that is handled.</typeparam>
public sealed class PreNotificationHandlerBehavior<TNotification> : INotificationPipelineBehavior<TNotification>
    where TNotification : INotification
{
    private readonly List<IPreNotificationHandler<TNotification>> _handlers;
    private readonly ILogger<PreNotificationHandlerBehavior<TNotification>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreNotificationHandlerBehavior{TNotification}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPreNotificationHandler{TNotification}"/>s.</param>
    /// <param name="logger">The <see cref="ILogger{TCategory}"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PreNotificationHandlerBehavior(
        IEnumerable<IPreNotificationHandler<TNotification>> handlers,
        ILogger<PreNotificationHandlerBehavior<TNotification>> logger)
    {
        Ensure.Arg.NotNull(handlers);
        Ensure.Arg.NotNull(logger);

        _handlers = handlers.ToList();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        INotificationContext<TNotification> context,
        NotificationPipelineDelegate<TNotification> next,
        CancellationToken cancellationToken = default)
    {
        foreach (IPreNotificationHandler<TNotification> handler in _handlers)
        {
            _logger.InvokingPreNotificationHandler(typeof(TNotification), handler.GetType());

            await handler.OnHandlingAsync(context, cancellationToken)
                         .ConfigureAwait(false);
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);
    }
}