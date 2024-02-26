// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a provider to get the <see cref="IPrincipal"/> which invokes the current request.
/// </summary>
public interface IRequestPrincipalProvider
{
    /// <summary>
    /// Gets the current <see cref="IPrincipal"/>.
    /// </summary>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    /// <returns>The current <see cref="IPrincipal"/>.</returns>
    IPrincipal? GetUser(IRequestContext context);
}