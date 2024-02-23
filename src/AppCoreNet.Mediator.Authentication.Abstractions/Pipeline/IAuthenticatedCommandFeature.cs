// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Security.Principal;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Provides the command authentication feature.
/// </summary>
public interface IAuthenticatedCommandFeature
{
    /// <summary>
    /// Gets the current <see cref="IPrincipal"/>.
    /// </summary>
    IPrincipal User { get; }
}