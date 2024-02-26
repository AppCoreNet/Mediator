// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a factory for <see cref="IEventPipeline"/> instances.
/// </summary>
public interface IEventPipelineFactory
{
    /// <summary>
    /// Resolves an <see cref="IEventPipeline"/> for the specified <paramref name="event"/>.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <returns>The <see cref="IEventPipeline"/> instance.</returns>
    IEventPipeline CreatePipeline(IEvent @event);
}