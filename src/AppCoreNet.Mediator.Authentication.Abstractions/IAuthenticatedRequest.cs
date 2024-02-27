// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a request which requires the current principal.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IAuthenticatedRequest<out TResponse> : IRequest<TResponse>
{
    /// <summary>
    /// Gets the <see cref="IPrincipal"/> used to authorize the request.
    /// </summary>
    IPrincipal User { get; }
}

/// <summary>
/// Represents a request which requires the current principal.
/// </summary>
public interface IAuthenticatedRequest : IAuthenticatedRequest<VoidResult>
{
}