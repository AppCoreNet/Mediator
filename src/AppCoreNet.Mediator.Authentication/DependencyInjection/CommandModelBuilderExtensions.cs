// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;
using AppCoreNet.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions to register the command model.
/// </summary>
public static class CommandModelBuilderExtensions
{
    /// <summary>
    /// Registers request authentication pipeline behavior.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    public static IMediatorBuilder AddCommandAuthentication(this IMediatorBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        builder.Services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<ICommandPrincipalProvider, CommandPrincipalProvider>(),
                ServiceDescriptor.Singleton<ICommandMetadataProvider, AuthorizedCommandMetadataProvider>(),
                ServiceDescriptor.Transient(typeof(ICommandPipelineBehavior<,>), typeof(AuthenticatedCommandBehavior<,>)),
            });

        return builder;
    }
}