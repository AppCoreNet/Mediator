// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a provider to get the <see cref="IPrincipal"/> which invokes the current command.
/// </summary>
public interface ICommandPrincipalProvider
{
    /// <summary>
    /// Gets the current <see cref="IPrincipal"/>.
    /// </summary>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    /// <returns>The current <see cref="IPrincipal"/>.</returns>
    IPrincipal? GetUser(ICommandContext context);
}