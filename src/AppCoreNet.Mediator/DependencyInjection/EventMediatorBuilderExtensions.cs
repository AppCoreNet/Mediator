// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

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
/// Provides extension methods to register event handlers and behaviors.
/// </summary>
public static class EventMediatorBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="IEventContextAccessor"/> with the DI container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    public static IMediatorBuilder AddEventContextAccessor(this IMediatorBuilder builder)
    {
        Ensure.Arg.NotNull(builder);
        builder.Services.TryAddSingleton<IEventContextAccessor, EventContextAccessor>();
        return builder;
    }

    /// <summary>
    /// Adds event handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IEventHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds event handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IEventHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds event pre-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPreHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPreEventHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds event pre-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPreHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPreEventHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds event post-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPostHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPostEventHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds event post-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPostHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPostEventHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds event pipeline behavior to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddBehavior(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IEventPipelineBehavior<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds event pipeline behaviors to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IMediatorBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddBehaviorsFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IEventPipelineBehavior<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }
}