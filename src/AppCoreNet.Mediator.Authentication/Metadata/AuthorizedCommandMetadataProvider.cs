// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.CommandModel.Pipeline;

namespace AppCore.CommandModel.Metadata;

/// <summary>
/// Represents a metadata provider for authorized commands.
/// </summary>
public class AuthorizedCommandMetadataProvider : ICommandMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type commandType, IDictionary<string, object> metadata)
    {
        bool requiresAuthentication = commandType.GetTypeInfo()
                                                 .GetCustomAttribute<AuthorizeAttribute>() != null;

        if (requiresAuthentication)
            metadata.Add(AuthenticatedCommandBehavior.IsAuthorizedMetadataKey, true);
    }
}