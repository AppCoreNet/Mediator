// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a command pipeline.
/// </summary>
/// <typeparam name="TResult">The result of the command.</typeparam>
public interface ICommandPipeline<TResult>
{
    /// <summary>
    /// Processes a command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous event operation.</returns>
    Task<TResult> InvokeAsync(ICommand<TResult> command, CancellationToken cancellationToken = default);
}