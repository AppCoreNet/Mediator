// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements command authentication support.
/// </summary>
public class AuthenticatedCommandFeature : IAuthenticatedCommandFeature
{
    /// <inheritdoc />
    public IPrincipal User { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedCommandFeature"/> class.
    /// </summary>
    /// <param name="user">The current <see cref="IPrincipal"/>.</param>
    public AuthenticatedCommandFeature(IPrincipal user)
    {
        Ensure.Arg.NotNull(user);
        User = user;
    }
}