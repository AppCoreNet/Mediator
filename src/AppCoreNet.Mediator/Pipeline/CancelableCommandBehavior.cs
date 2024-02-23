// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

internal static class CancelableCommandBehavior
{
    internal const string IsCancelableMetadataKey = "IsCancelable";
}

/// <summary>
/// Implements cancellation support for commands. The command must be decorated with the
/// <see cref="CancelableAttribute"/>.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class CancelableCommandBehavior<TCommand, TResult> : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <inheritdoc />
    public async Task ProcessAsync(
        ICommandContext<TCommand, TResult> context,
        CommandPipelineDelegate<TCommand, TResult> next,
        CancellationToken cancellationToken)
    {
        bool isCancelable = context.CommandDescriptor.GetMetadata(
            CancelableCommandBehavior.IsCancelableMetadataKey,
            false);

        if (isCancelable)
        {
            var cts = new CancellationTokenSource();
            cancellationToken.Register(() => cts.Cancel());

            context.AddFeature<ICancelableCommandFeature>(new CancelableCommandFeature(cts));
            cancellationToken = cts.Token;
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
    }
}