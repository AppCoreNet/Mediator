// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which provides metadata for notifications.
/// </summary>
public interface INotificationMetadataProvider
{
    /// <summary>
    /// Gets the metadata for the specified <paramref name="notificationType"/>.
    /// </summary>
    /// <param name="notificationType">The type of the notification.</param>
    /// <param name="metadata">The dictionary which should be populated with metadata.</param>
    void GetMetadata(Type notificationType, IDictionary<string, object> metadata);
}