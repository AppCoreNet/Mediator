// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Creates new <see cref="CommandDescriptor"/> instances.
/// </summary>
public class CommandDescriptorFactory : ICommandDescriptorFactory
{
    private readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, object>> _metadataCache = new ();

    private readonly IEnumerable<ICommandMetadataProvider> _metadataProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDescriptorFactory"/> class.
    /// </summary>
    /// <param name="metadataProviders">The <see cref="IEnumerable{T}"/> of <see cref="ICommandMetadataProvider"/>'s.</param>
    public CommandDescriptorFactory(IEnumerable<ICommandMetadataProvider> metadataProviders)
    {
        Ensure.Arg.NotNull(metadataProviders);
        _metadataProviders = metadataProviders;
    }

    private IReadOnlyDictionary<string, object> GetMetadata(Type commandType)
    {
        return _metadataCache.GetOrAdd(
            commandType,
            t =>
            {
                var metadata = new Dictionary<string, object>();
                foreach (ICommandMetadataProvider metadataProvider in _metadataProviders)
                {
                    metadataProvider.GetMetadata(t, metadata);
                }

                return new ReadOnlyDictionary<string, object>(metadata);
            });
    }

    /// <inheritdoc />
    public CommandDescriptor CreateDescriptor(Type commandType)
    {
        Ensure.Arg.NotNull(commandType);
        Ensure.Arg.OfType(commandType, typeof(ICommand<>));

        return new CommandDescriptor(commandType, GetMetadata(commandType));
    }
}