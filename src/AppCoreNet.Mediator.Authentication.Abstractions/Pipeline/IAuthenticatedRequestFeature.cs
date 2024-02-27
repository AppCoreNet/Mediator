// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides the request authentication feature.
/// </summary>
public interface IAuthenticatedRequestFeature
{
    /// <summary>
    /// Gets the current <see cref="IPrincipal"/>.
    /// </summary>
    IPrincipal User { get; }
}