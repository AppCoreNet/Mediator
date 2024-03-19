// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator;
using AppCoreNet.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register notification handlers and behaviors.
/// </summary>
public static class NotificationMediatorBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="INotificationContextAccessor"/> with the DI container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    public static IMediatorBuilder AddNotificationContextAccessor(this IMediatorBuilder builder)
    {
        Ensure.Arg.NotNull(builder);
        builder.Services.TryAddSingleton<INotificationContextAccessor, NotificationContextAccessor>();
        return builder;
    }

    /// <summary>
    /// Adds notification handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the notification.</param>
    /// <param name="lifetime">The lifetime of the notification.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddNotificationHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(INotificationHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds notification handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddNotificationHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(INotificationHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds notification pre-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPreNotificationHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPreNotificationHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds notification pre-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPreNotificationHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPreNotificationHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds notification post-handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPostNotificationHandler(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(IPostNotificationHandler<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds notification post-handlers to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddPostNotificationHandlersFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(IPostNotificationHandler<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }

    /// <summary>
    /// Adds notification pipeline behavior to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddNotificationBehavior(
        this IMediatorBuilder builder,
        Type handlerType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(handlerType);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Describe(typeof(INotificationPipelineBehavior<>), handlerType, lifetime));

        return builder;
    }

    /// <summary>
    /// Adds notification pipeline behaviors to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IMediatorBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The passed <see cref="IMediatorBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IMediatorBuilder AddNotificationBehaviorsFrom(
        this IMediatorBuilder builder,
        Action<IServiceDescriptorReflectionBuilder> configure,
        ServiceLifetime defaultLifetime = ServiceLifetime.Transient)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        builder.Services.TryAddEnumerableFrom(
            typeof(INotificationPipelineBehavior<>),
            r =>
            {
                r.WithDefaultLifetime(defaultLifetime);
                configure(r);
            });

        return builder;
    }
}