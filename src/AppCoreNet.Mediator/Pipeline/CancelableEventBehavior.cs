// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

internal static class CancelableEventBehavior
{
    internal const string IsCancelableMetadataKey = "IsCancelable";
}

/// <summary>
/// Implements cancellation support for events. The event must be decorated with the
/// <see cref="CancelableAttribute"/>.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public class CancelableEventBehavior<TEvent> : IEventPipelineBehavior<TEvent>
    where TEvent : IEvent
{
    /// <inheritdoc />
    public async Task HandleAsync(
        IEventContext<TEvent> context,
        EventPipelineDelegate<TEvent> next,
        CancellationToken cancellationToken)
    {
        bool isCancelable = context.EventDescriptor.GetMetadata(
            CancelableEventBehavior.IsCancelableMetadataKey,
            false);

        if (isCancelable)
        {
            var cts = new CancellationTokenSource();
            cancellationToken.Register(() => cts.Cancel());

            context.AddFeature<ICancelableEventFeature>(new CancelableEventFeature(cts));
            cancellationToken = cts.Token;
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
    }
}