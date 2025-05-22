// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements cancellation support for notifications. The notification must be decorated with the
/// <see cref="CancelableAttribute"/>.
/// </summary>
/// <typeparam name="TNotification">The type of the notification.</typeparam>
public class CancelableNotificationBehavior<TNotification> : INotificationPipelineBehavior<TNotification>
    where TNotification : INotification
{
    /// <inheritdoc />
    public async Task HandleAsync(
        INotificationContext<TNotification> context,
        NotificationPipelineDelegate<TNotification> next,
        CancellationToken cancellationToken = default)
    {
        bool isCancelable = context.NotificationDescriptor.GetMetadata(
            MetadataKeys.IsCancelable,
            false);

        CancellationTokenSource? cts = null;
        try
        {
            if (isCancelable)
            {
                cts = new CancellationTokenSource();

                // ReSharper disable once AccessToDisposedClosure
                cancellationToken.Register(() => cts.Cancel());

                context.AddFeature<ICancelableNotificationFeature>(new CancelableNotificationFeature(cts));
                cancellationToken = cts.Token;
            }

            await next(context, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            cts?.Dispose();
        }

        cancellationToken.ThrowIfCancellationRequested();
    }
}