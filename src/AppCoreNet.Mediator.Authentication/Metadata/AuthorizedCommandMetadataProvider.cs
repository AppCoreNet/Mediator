// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

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