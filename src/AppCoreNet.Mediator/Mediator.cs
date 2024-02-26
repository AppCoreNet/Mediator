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
    private readonly IEventPipelineFactory _eventPipelineFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="eventPipelineFactory">The event pipeline factory.</param>
    /// <param name="requestPipelineFactory">The command pipeline factory.</param>
    /// <exception cref="ArgumentNullException">Some argument is <c>null</c>.</exception>
    public Mediator(IRequestPipelineFactory requestPipelineFactory, IEventPipelineFactory eventPipelineFactory)
    {
        Ensure.Arg.NotNull(requestPipelineFactory);
        Ensure.Arg.NotNull(eventPipelineFactory);

        _requestPipelineFactory = requestPipelineFactory;
        _eventPipelineFactory = eventPipelineFactory;
    }

    /// <inheritdoc />
    public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        Ensure.Arg.NotNull(request);

        IRequestPipeline<TResponse> pipeline = _requestPipelineFactory.CreatePipeline(request);
        return await pipeline.InvokeAsync(request, cancellationToken)
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