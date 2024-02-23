// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a delegate in the command pipeline.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The result produced by the command.</typeparam>
/// <param name="command">The command.</param>
/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous command operation.</returns>
public delegate Task CommandPipelineDelegate<in TCommand, TResult>(
    ICommandContext<TCommand, TResult> command,
    CancellationToken cancellationToken)
    where TCommand : ICommand<TResult>;