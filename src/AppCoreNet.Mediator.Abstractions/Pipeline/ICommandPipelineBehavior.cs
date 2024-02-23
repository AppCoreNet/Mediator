// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Represents a behavior for the command pipeline.
/// </summary>
/// <typeparam name="TCommand">The type of the command that is handled.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Processes the specified command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="next">The behavior which should be invoked next.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task ProcessAsync(
        ICommandContext<TCommand, TResult> context,
        CommandPipelineDelegate<TCommand, TResult> next,
        CancellationToken cancellationToken);
}