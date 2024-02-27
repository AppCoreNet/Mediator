// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements request authentication support.
/// </summary>
public class AuthenticatedRequestFeature : IAuthenticatedRequestFeature
{
    /// <inheritdoc />
    public IPrincipal User { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedRequestFeature"/> class.
    /// </summary>
    /// <param name="user">The current <see cref="IPrincipal"/>.</param>
    public AuthenticatedRequestFeature(IPrincipal user)
    {
        Ensure.Arg.NotNull(user);
        User = user;
    }
}