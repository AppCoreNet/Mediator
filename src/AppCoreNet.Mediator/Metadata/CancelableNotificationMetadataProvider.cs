// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides metadata for cancelable notifications.
/// </summary>
public class CancelableNotificationMetadataProvider : INotificationMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type notificationType, IDictionary<string, object> metadata)
    {
        bool isCancelable = notificationType.GetTypeInfo()
                                     .GetCustomAttribute<CancelableAttribute>() != null;

        if (isCancelable)
            metadata.Add(MetadataKeys.IsCancelable, true);
    }
}