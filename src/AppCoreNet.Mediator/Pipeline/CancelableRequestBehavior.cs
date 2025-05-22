// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements cancellation support for request. The request must be decorated with the
/// <see cref="CancelableAttribute"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class CancelableRequestBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public async Task HandleAsync(
        IRequestContext<TRequest, TResponse> context,
        RequestPipelineDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken = default)
    {
        bool isCancelable = context.RequestDescriptor.GetMetadata(
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

                context.AddFeature<ICancelableRequestFeature>(new CancelableRequestFeature(cts));
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