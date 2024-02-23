// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Pipeline behavior which invokes <see cref="IPreCommandHandler{TCommand,TResult}"/>s when the command
/// is about to be handled.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class PreCommandHandlerBehavior<TCommand, TResult> : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<IPreCommandHandler<TCommand, TResult>> _handlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreCommandHandlerBehavior{TCommand,TResult}"/> class.
    /// </summary>
    /// <param name="handlers">An <see cref="IEnumerable{T}"/> of <see cref="IPreCommandHandler{TCommand,TResult}"/>s.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlers"/> is <c>null</c>.</exception>
    public PreCommandHandlerBehavior(IEnumerable<IPreCommandHandler<TCommand, TResult>> handlers)
    {
        Ensure.Arg.NotNull(handlers);
        _handlers = handlers;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(
        ICommandContext<TCommand, TResult> context,
        CommandPipelineDelegate<TCommand, TResult> next,
        CancellationToken cancellationToken)
    {
        if (!context.IsCompleted)
        {
            foreach (IPreCommandHandler<TCommand, TResult> handler in _handlers)
            {
                await handler.OnHandlingAsync(context, cancellationToken)
                             .ConfigureAwait(false);
            }
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);
    }
}