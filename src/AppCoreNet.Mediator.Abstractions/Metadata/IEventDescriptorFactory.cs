// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which instantiates <see cref="EventDescriptor"/> objects.
/// </summary>
public interface IEventDescriptorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="EventDescriptor"/> for the specified <paramref name="eventType"/>.
    /// </summary>
    /// <param name="eventType">The type of the event.</param>
    /// <returns>The <see cref="EventDescriptor"/>.</returns>
    EventDescriptor CreateDescriptor(Type eventType);
}