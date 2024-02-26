// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPostCommandHandler{TCommand,TResult}"/>s when the
/// command has been successfully handled.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public sealed class PostCommandHandlerBehavior<TCommand, TResult> : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<IPostCommandHandler<TCommand, TResult>> _handlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostCommandHandlerBehavior{TCommand,TResult}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPostCommandHandler{TCommand,TResult}"/>s.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PostCommandHandlerBehavior(IEnumerable<IPostCommandHandler<TCommand, TResult>> handlers)
    {
        Ensure.Arg.NotNull(handlers);
        _handlers = handlers;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        ICommandContext<TCommand, TResult> context,
        CommandPipelineDelegate<TCommand, TResult> next,
        CancellationToken cancellationToken = default)
    {
        await next(context, cancellationToken)
            .ConfigureAwait(false);

        if (!context.IsFailed)
        {
            foreach (IPostCommandHandler<TCommand, TResult> handler in _handlers)
            {
                await handler.OnHandledAsync(context, cancellationToken)
                             .ConfigureAwait(false);
            }
        }
    }
}