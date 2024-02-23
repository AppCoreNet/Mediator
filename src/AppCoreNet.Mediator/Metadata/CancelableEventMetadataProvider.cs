// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides metadata for cancelable events.
/// </summary>
public class CancelableEventMetadataProvider : IEventMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type eventType, IDictionary<string, object> metadata)
    {
        bool isCancelable = eventType.GetTypeInfo()
                                     .GetCustomAttribute<CancelableAttribute>() != null;

        if (isCancelable)
            metadata.Add(CancelableEventBehavior.IsCancelableMetadataKey, true);
    }
}