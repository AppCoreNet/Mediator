// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides metadata for cancelable requests.
/// </summary>
public class CancelableRequestMetadataProvider : IRequestMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type requestType, IDictionary<string, object> metadata)
    {
        bool isCancelable = requestType.GetTypeInfo()
                                       .GetCustomAttribute<CancelableAttribute>() != null;

        if (isCancelable)
            metadata.Add(CancelableRequestBehavior.IsCancelableMetadataKey, true);
    }
}