// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Security.Principal;

namespace AppCore.CommandModel.Pipeline;

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