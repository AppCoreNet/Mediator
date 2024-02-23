// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents the command processor.
/// </summary>
public interface ICommandProcessor
{
    /// <summary>
    /// Processes a command.
    /// </summary>
    /// <param name="command">The command to process.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
}