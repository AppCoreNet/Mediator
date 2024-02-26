// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a metadata provider for authorized requests.
/// </summary>
public class AuthorizedRequestMetadataProvider : IRequestMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type requestType, IDictionary<string, object> metadata)
    {
        bool requiresAuthentication = requestType.GetTypeInfo()
                                                 .GetCustomAttribute<AuthorizeAttribute>() != null;

        if (requiresAuthentication)
            metadata.Add(AuthenticatedRequestBehavior.IsAuthorizedMetadataKey, true);
    }
}