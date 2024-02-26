// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which provides metadata for requests.
/// </summary>
public interface IRequestMetadataProvider
{
    /// <summary>
    /// Gets the metadata for the specified <paramref name="requestType"/>.
    /// </summary>
    /// <param name="requestType">The type of the request.</param>
    /// <param name="metadata">The dictionary which should be populated with metadata.</param>
    void GetMetadata(Type requestType, IDictionary<string, object> metadata);
}