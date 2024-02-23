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
/// Provides extensions to register the event model.
/// </summary>
public static class EventModelBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="IEventContextAccessor"/> with the DI container.
    /// </summary>
    /// <returns>The <see cref="EventModelBuilderExtensions"/>.</returns>
    public static IEventModelBuilder AddEventContextAccessor(this IEventModelBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        builder.Services.TryAddSingleton<IEventContextAccessor, EventContextAccessor>();
        return builder;
    }

    /// <summary>
    /// Adds event handler to the container.
    /// </summary>
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddHandler(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddHandlersFrom(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddPreHandler(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddPreHandlersFrom(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddPostHandler(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddPostHandlersFrom(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="handlerType">The type of the handler.</param>
    /// <param name="lifetime">The lifetime of the handler.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="handlerType"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddBehavior(
        this IEventModelBuilder builder,
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
    /// <param name="builder">The <see cref="IEventModelBuilder"/>.</param>
    /// <param name="configure">The delegate used to configure the registration sources.</param>
    /// <param name="defaultLifetime">The default handler lifetime.</param>
    /// <returns>The <see cref="IEventModelBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is <c>null</c>.</exception>
    public static IEventModelBuilder AddBehaviorsFrom(
        this IEventModelBuilder builder,
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