﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Creates new <see cref="EventDescriptor"/> instances.
/// </summary>
public class EventDescriptorFactory : IEventDescriptorFactory
{
    private readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, object>> _metadataCache = new();

    private readonly IEnumerable<IEventMetadataProvider> _metadataProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDescriptorFactory"/> class.
    /// </summary>
    /// <param name="metadataProviders">The <see cref="IEnumerable{T}"/> of <see cref="IEventMetadataProvider"/>'s.</param>
    public EventDescriptorFactory(IEnumerable<IEventMetadataProvider> metadataProviders)
    {
        Ensure.Arg.NotNull(metadataProviders);
        _metadataProviders = metadataProviders;
    }

    private IReadOnlyDictionary<string, object> GetMetadata(Type eventType)
    {
        return _metadataCache.GetOrAdd(
            eventType,
            t =>
            {
                var metadata = new Dictionary<string, object>();
                foreach (IEventMetadataProvider eventMetadataProvider in _metadataProviders)
                {
                    eventMetadataProvider.GetMetadata(t, metadata);
                }

                return new ReadOnlyDictionary<string, object>(metadata);
            });
    }

    /// <inheritdoc />
    public EventDescriptor CreateDescriptor(Type eventType)
    {
        Ensure.Arg.NotNull(eventType);
        Ensure.Arg.OfType<IEvent>(eventType);

        return new EventDescriptor(eventType, GetMetadata(eventType));
    }
}