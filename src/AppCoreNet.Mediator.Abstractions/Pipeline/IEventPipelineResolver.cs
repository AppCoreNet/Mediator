// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a resolver for <see cref="IEventPipeline"/> instances.
/// </summary>
public interface IEventPipelineResolver
{
    /// <summary>
    /// Resolves an <see cref="IEventPipeline{TEvent}"/> for the specified <paramref name="eventType"/>.
    /// </summary>
    /// <param name="eventType">The type of the event.</param>
    /// <returns>The <see cref="IEventPipeline{TEvent}"/> instance.</returns>
    IEventPipeline Resolve(Type eventType);
}