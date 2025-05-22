// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Creates new <see cref="RequestDescriptor"/> instances.
/// </summary>
public class RequestDescriptorFactory : IRequestDescriptorFactory
{
    private readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, object>> _metadataCache = new();
    private readonly IEnumerable<IRequestMetadataProvider> _metadataProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestDescriptorFactory"/> class.
    /// </summary>
    /// <param name="metadataProviders">The <see cref="IEnumerable{T}"/> of <see cref="IRequestMetadataProvider"/>'s.</param>
    public RequestDescriptorFactory(IEnumerable<IRequestMetadataProvider> metadataProviders)
    {
        Ensure.Arg.NotNull(metadataProviders);
        _metadataProviders = metadataProviders;
    }

    private IReadOnlyDictionary<string, object> GetMetadata(Type requestType)
    {
        return _metadataCache.GetOrAdd(
            requestType,
            t =>
            {
                var metadata = new Dictionary<string, object>();
                foreach (IRequestMetadataProvider metadataProvider in _metadataProviders)
                {
                    metadataProvider.GetMetadata(t, metadata);
                }

                return new ReadOnlyDictionary<string, object>(metadata);
            });
    }

    /// <inheritdoc />
    public RequestDescriptor CreateDescriptor(Type requestType)
    {
        Ensure.Arg.NotNull(requestType);
        Ensure.Arg.OfGenericType(requestType, typeof(IRequest<>));

        return new RequestDescriptor(requestType, GetMetadata(requestType));
    }
}