// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides metadata for cancelable commands.
/// </summary>
public class CancelableCommandMetadataProvider : ICommandMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type commandType, IDictionary<string, object> metadata)
    {
        bool isCancelable = commandType.GetTypeInfo()
                                       .GetCustomAttribute<CancelableAttribute>() != null;

        if (isCancelable)
            metadata.Add(CancelableCommandBehavior.IsCancelableMetadataKey, true);
    }
}