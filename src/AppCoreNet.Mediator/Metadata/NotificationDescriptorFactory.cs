// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Creates new <see cref="NotificationDescriptor"/> instances.
/// </summary>
public class NotificationDescriptorFactory : INotificationDescriptorFactory
{
    private readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, object>> _metadataCache = new ();
    private readonly IEnumerable<INotificationMetadataProvider> _metadataProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationDescriptorFactory"/> class.
    /// </summary>
    /// <param name="metadataProviders">The <see cref="IEnumerable{T}"/> of <see cref="INotificationMetadataProvider"/>'s.</param>
    public NotificationDescriptorFactory(IEnumerable<INotificationMetadataProvider> metadataProviders)
    {
        Ensure.Arg.NotNull(metadataProviders);
        _metadataProviders = metadataProviders;
    }

    private IReadOnlyDictionary<string, object> GetMetadata(Type notificationType)
    {
        return _metadataCache.GetOrAdd(
            notificationType,
            t =>
            {
                var metadata = new Dictionary<string, object>();
                foreach (INotificationMetadataProvider metadataProvider in _metadataProviders)
                {
                    metadataProvider.GetMetadata(t, metadata);
                }

                return new ReadOnlyDictionary<string, object>(metadata);
            });
    }

    /// <inheritdoc />
    public NotificationDescriptor CreateDescriptor(Type notificationType)
    {
        Ensure.Arg.NotNull(notificationType);
        Ensure.Arg.OfType<INotification>(notificationType);

        return new NotificationDescriptor(notificationType, GetMetadata(notificationType));
    }
}