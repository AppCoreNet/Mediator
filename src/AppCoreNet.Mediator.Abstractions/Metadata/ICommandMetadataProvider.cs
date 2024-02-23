// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which provides metadata for commands.
/// </summary>
public interface ICommandMetadataProvider
{
    /// <summary>
    /// Gets the metadata for the specified <paramref name="commandType"/>.
    /// </summary>
    /// <param name="commandType">The type of the command.</param>
    /// <param name="metadata">The dictionary which should be populated with metadata.</param>
    void GetMetadata(Type commandType, IDictionary<string, object> metadata);
}