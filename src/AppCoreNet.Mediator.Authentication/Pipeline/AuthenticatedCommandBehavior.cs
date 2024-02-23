// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

internal class AuthenticatedCommandBehavior
{
    internal const string IsAuthorizedMetadataKey = "IsAuthorized";
}

/// <summary>
/// Implements authentication support for commands. The command must be decorated with the
/// <see cref="AuthorizeAttribute"/>. The current user is obtained using implementations
/// of <see cref="ICommandPrincipalProvider"/>.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class AuthenticatedCommandBehavior<TCommand, TResult> : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<ICommandPrincipalProvider> _principalProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedCommandBehavior{TCommand,TResult}"/>.
    /// </summary>
    /// <param name="principalProviders">The command principal providers.</param>
    public AuthenticatedCommandBehavior(IEnumerable<ICommandPrincipalProvider> principalProviders)
    {
        Ensure.Arg.NotNull(principalProviders);
        _principalProviders = principalProviders;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(
        ICommandContext<TCommand, TResult> context,
        CommandPipelineDelegate<TCommand, TResult> next,
        CancellationToken cancellationToken
    )
    {
        IPrincipal principal = _principalProviders.Select(p => p.GetUser(context))
                                                  .FirstOrDefault(p => p != null) ?? new ClaimsPrincipal(new ClaimsIdentity());

        context.AddFeature<IAuthenticatedCommandFeature>(new AuthenticatedCommandFeature(principal));

        if (!context.IsCompleted)
        {
            bool isAuthorized =
                context.CommandDescriptor.GetMetadata(
                    AuthenticatedCommandBehavior.IsAuthorizedMetadataKey,
                    false);

            if (isAuthorized && !context.IsAuthenticated())
            {
                context.Fail(new AuthorizationException());
            }
        }

        await next(context, cancellationToken)
            .ConfigureAwait(false);
    }
}