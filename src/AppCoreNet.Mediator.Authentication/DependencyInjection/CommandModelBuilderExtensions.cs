// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

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
    /// Registers command authentication pipeline behavior.
    /// </summary>
    /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddAuthentication(this ICommandModelBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        builder.Services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<ICommandPrincipalProvider, CommandPrincipalProvider>(),
                ServiceDescriptor.Singleton<ICommandMetadataProvider, AuthorizedCommandMetadataProvider>(),
                ServiceDescriptor.Transient(
                    typeof(ICommandPipelineBehavior<,>),
                    typeof(AuthenticatedCommandBehavior<,>))
            });

        return builder;
    }
}