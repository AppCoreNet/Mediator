// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a factory for <see cref="IRequestPipeline{TResponse}"/> instances.
/// </summary>
public interface IRequestPipelineFactory
{
    /// <summary>
    /// Resolves an <see cref="IRequestPipeline{TResponse}"/> for the specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <typeparam name="TResponse">The response of the request.</typeparam>
    /// <returns>The <see cref="IRequestPipeline{TResponse}"/> instance.</returns>
    IRequestPipeline<TResponse> CreatePipeline<TResponse>(IRequest<TResponse> request);
}