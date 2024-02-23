// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Represents a handler which is invoked after a command was successfully processed.
/// </summary>
/// <typeparam name="TCommand">The type of the command that is handled.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IPostCommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Invoked after the command has been processed.
    /// </summary>
    /// <param name="context">The context of the command that was processed.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task OnHandledAsync(
        ICommandContext<TCommand, TResult> context,
        CancellationToken cancellationToken);
}