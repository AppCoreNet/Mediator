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
    private readonly IRequestPipelineFactory _requestPipelineFactory;
    private readonly INotificationPipelineFactory _notificationPipelineFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="notificationPipelineFactory">The notification pipeline factory.</param>
    /// <param name="requestPipelineFactory">The request pipeline factory.</param>
    /// <exception cref="ArgumentNullException">Some argument is <c>null</c>.</exception>
    public Mediator(
        IRequestPipelineFactory requestPipelineFactory,
        INotificationPipelineFactory notificationPipelineFactory)
    {
        Ensure.Arg.NotNull(requestPipelineFactory);
        Ensure.Arg.NotNull(notificationPipelineFactory);

        _requestPipelineFactory = requestPipelineFactory;
        _notificationPipelineFactory = notificationPipelineFactory;
    }

    /// <inheritdoc />
    public async Task<TResponse> RequestAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(request);

        IRequestPipeline<TResponse> pipeline = _requestPipelineFactory.CreatePipeline(request);
        return await pipeline.InvokeAsync(request, cancellationToken)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(notification);

        INotificationPipeline pipeline = _notificationPipelineFactory.CreatePipeline(notification);
        await pipeline.InvokeAsync(notification, cancellationToken)
                      .ConfigureAwait(false);
    }
}