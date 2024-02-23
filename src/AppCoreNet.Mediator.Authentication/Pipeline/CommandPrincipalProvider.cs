// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Security.Principal;
using AppCoreNet.Diagnostics;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Retrieves the principal from the current command. The command is required to
/// implement <see cref="IAuthenticatedCommand{TResult}"/>.
/// </summary>
public class CommandPrincipalProvider : ICommandPrincipalProvider
{
    /// <inheritdoc />
    public IPrincipal? GetUser(ICommandContext context)
    {
        Ensure.Arg.NotNull(context);
        return (context.Command as IAuthenticatedCommand<object>)?.User;
    }
}