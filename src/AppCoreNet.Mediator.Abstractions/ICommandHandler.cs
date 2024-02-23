// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator;

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