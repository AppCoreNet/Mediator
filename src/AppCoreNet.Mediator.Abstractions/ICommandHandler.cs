// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.CommandModel;

/// <summary>
/// Provides a handler for commands.
/// </summary>
/// <typeparam name="TCommand">The type of the command that is handled.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command">The command that should be handled.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}