// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection;
using AppCoreNet.Mediator;
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
    /// Registers the <see cref="ICommandContextAccessor"/> with the DI container.
    /// </summary>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    public static ICommandModelBuilder AddCommandContext(this ICommandModelBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        builder.Services.TryAddSingleton<ICommandContextAccessor, CommandContextAccessor>();
        return builder;
    }

    /// <summary>
    /// Adds command handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddHandler(
        this ICommandModelBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(ICommandHandler<,>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds command handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddHandlersFrom(
        this ICommandModelBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(ICommandHandler<,>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds command pre-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddPreHandler(
        this ICommandModelBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPreCommandHandler<,>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds command pre-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddPreHandlersFrom(
        this ICommandModelBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPreCommandHandler<,>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds command post-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddPostHandler(
        this ICommandModelBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPostCommandHandler<,>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds command post-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddPostHandlersFrom(
        this ICommandModelBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPostCommandHandler<,>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds command pipeline behavior to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddBehavior(
        this ICommandModelBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(ICommandPipelineBehavior<,>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds command pipeline behaviors to the container.
    /// </summary>
    /// <param name="builder">The <see cref="ICommandModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="ICommandModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static ICommandModelBuilder AddBehaviorsFrom(
        this ICommandModelBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(ICommandPipelineBehavior<,>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }
}