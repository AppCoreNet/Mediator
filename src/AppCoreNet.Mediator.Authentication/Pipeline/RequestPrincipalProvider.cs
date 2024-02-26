// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Retrieves the principal from the current request. The request is required to
/// implement <see cref="IAuthenticatedRequest{TResponse}"/>.
/// </summary>
public class RequestPrincipalProvider : IRequestPrincipalProvider
{
    /// <inheritdoc />
    public IPrincipal? GetUser(IRequestContext context)
    {
        Ensure.Arg.NotNull(context);
        return (context.Request as IAuthenticatedRequest<object>)?.User;
    }
}