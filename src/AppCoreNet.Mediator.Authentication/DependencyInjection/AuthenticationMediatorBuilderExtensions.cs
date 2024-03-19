// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;
using AppCoreNet.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions to register request authentication.
/// </summary>
public static class AuthenticationMediatorBuilderExtensions
{
    /// <summary>
    /// Registers request authentication pipeline behavior.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    public static IMediatorBuilder AddRequestAuthentication(this IMediatorBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        builder.Services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<IRequestPrincipalProvider, RequestPrincipalProvider>(),
                ServiceDescriptor.Singleton<IRequestMetadataProvider, AuthorizedRequestMetadataProvider>(),
                ServiceDescriptor.Transient(typeof(IRequestPipelineBehavior<,>), typeof(AuthenticatedRequestBehavior<,>)),
            });

        return builder;
    }
}