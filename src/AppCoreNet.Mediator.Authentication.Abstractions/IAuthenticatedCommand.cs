// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Principal;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a command which requires the current principal.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IAuthenticatedCommand<out TResult> : ICommand<TResult>
{
    /// <summary>
    /// Gets the <see cref="IPrincipal"/> used to authorize the command.
    /// </summary>
    IPrincipal User { get; }
}

/// <summary>
/// Represents a command which requires the current principal.
/// </summary>
public interface IAuthenticatedCommand : IAuthenticatedCommand<VoidResult>
{
}