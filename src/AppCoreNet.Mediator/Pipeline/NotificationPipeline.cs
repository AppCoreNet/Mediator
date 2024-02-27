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

public sealed class NotificationPipeline<TNotification> : INotificationPipeline
    where TNotification : INotification
{
    private readonly INotificationDescriptorFactory _descriptorFactory;
    private readonly List<INotificationPipelineBehavior<TNotification>> _behaviors;
    private readonly List<INotificationHandler<TNotification>> _handlers;
    private readonly ILogger<NotificationPipeline<TNotification>> _logger;
    private readonly INotificationContextAccessor? _contextAccessor;

    public NotificationPipeline(
        INotificationDescriptorFactory descriptorFactory,
        IEnumerable<INotificationPipelineBehavior<TNotification>> behaviors,
        IEnumerable<INotificationHandler<TNotification>> handlers,
        ILogger<NotificationPipeline<TNotification>> logger,
        INotificationContextAccessor? contextAccessor = null)
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
    public async Task InvokeAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(notification);

        NotificationDescriptor descriptor = _descriptorFactory.CreateDescriptor(typeof(TNotification));
        var context = new NotificationContext<TNotification>(descriptor, (TNotification)notification);

        if (_contextAccessor != null)
            _contextAccessor.CurrentContext = context;

        try
        {
            await InvokeAsync(context, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (_contextAccessor != null)
                _contextAccessor.CurrentContext = null;
        }
    }

    private async Task InvokeAsync(INotificationContext<TNotification> context, CancellationToken cancellationToken)
    {
        bool handlerInvoked = false;
        async Task Handler(INotificationContext<TNotification> c, CancellationToken ct)
        {
            handlerInvoked = true;
            foreach (INotificationHandler<TNotification> handler in _handlers)
            {
                await handler.HandleAsync(c.Notification, ct);
                ct.ThrowIfCancellationRequested();
            }
        }

        _logger.PipelineProcessing(typeof(TNotification));

        var stopwatch = Stopwatch.StartNew();

        try
        {
            INotificationPipelineBehavior<TNotification>? current = null;
            await ((IEnumerable<INotificationPipelineBehavior<TNotification>>)_behaviors)
                  .Reverse()
                  .Aggregate(
                      (NotificationPipelineDelegate<TNotification>)Handler,
                      (next, behavior) => async (e, ct) =>
                      {
                          _logger.InvokingBehavior(typeof(TNotification), behavior.GetType());
                          current = behavior;
                          await behavior.HandleAsync(e, next, ct);
                      })(context, cancellationToken)
                  .ConfigureAwait(false);

            if (handlerInvoked)
            {
                _logger.PipelineProcessed(typeof(TNotification), stopwatch.Elapsed);
            }
            else
            {
                _logger.PipelineShortCircuited(typeof(TNotification), current!.GetType(), stopwatch.Elapsed);
            }
        }
        catch (Exception error)
        {
            _logger.PipelineFailed(typeof(TNotification), stopwatch.Elapsed, error);
            throw;
        }
    }
}