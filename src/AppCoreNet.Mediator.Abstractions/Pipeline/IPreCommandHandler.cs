// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a handler which is invoked before a command is processed.
/// </summary>
/// <typeparam name="TCommand">The type of the command that is handled.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IPreCommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Invoked before the command is being processed.
    /// </summary>
    /// <param name="context">The context of the command that is about to be processed.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command operation.</returns>
    Task OnHandlingAsync(ICommandContext<TCommand, TResult> context, CancellationToken cancellationToken = default);
}