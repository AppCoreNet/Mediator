// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a factory for <see cref="ICommandPipeline{TResult}"/> instances.
/// </summary>
public interface ICommandPipelineFactory
{
    /// <summary>
    /// Resolves an <see cref="ICommandPipeline{TResult}"/> for the specified <paramref name="command"/>.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <typeparam name="TResult">The result of the command.</typeparam>
    /// <returns>The <see cref="ICommandPipeline{TResult}"/> instance.</returns>
    ICommandPipeline<TResult> CreatePipeline<TResult>(ICommand<TResult> command);
}