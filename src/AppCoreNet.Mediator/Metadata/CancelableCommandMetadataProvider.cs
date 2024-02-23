// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.CommandModel.Pipeline;
using AppCore.EventModel;

namespace AppCore.CommandModel.Metadata;

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