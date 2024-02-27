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

internal class AuthenticatedRequestBehavior
{
    internal const string IsAuthorizedMetadataKey = "IsAuthorized";
}

/// <summary>
/// Implements authentication support for request. The request must be decorated with the
/// <see cref="AuthorizeAttribute"/>. The current user is obtained using implementations
/// of <see cref="IRequestPrincipalProvider"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class AuthenticatedRequestBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IRequestPrincipalProvider> _principalProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedRequestBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="principalProviders">The request principal providers.</param>
    public AuthenticatedRequestBehavior(IEnumerable<IRequestPrincipalProvider> principalProviders)
    {
        Ensure.Arg.NotNull(principalProviders);
        _principalProviders = principalProviders;
    }

    /// <inheritdoc />
    public async Task HandleAsync(
        IRequestContext<TRequest, TResponse> context,
        RequestPipelineDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        IPrincipal principal = _principalProviders.Select(p => p.GetUser(context))
                                                  .FirstOrDefault(p => p != null)
                               ?? new ClaimsPrincipal(new ClaimsIdentity());

        context.AddFeature<IAuthenticatedRequestFeature>(new AuthenticatedRequestFeature(principal));

        if (!context.IsCompleted)
        {
            bool isAuthorized =
                context.RequestDescriptor.GetMetadata(
                    AuthenticatedRequestBehavior.IsAuthorizedMetadataKey,
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