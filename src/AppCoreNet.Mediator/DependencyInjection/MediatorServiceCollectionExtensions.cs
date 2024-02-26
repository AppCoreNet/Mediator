// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator;
using AppCoreNet.Mediator.Metadata;
using AppCoreNet.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to configure the <see cref="IMediatorBuilder"/>.
/// </summary>
public static class MediatorServiceCollectionExtensions
{
    /// <summary>
    /// Adds the mediator to the DI container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IMediatorBuilder"/> used to configure the mediator.</returns>
    public static IMediatorBuilder AddMediator(this IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);

        services.TryAdd(
            new[]
            {
                ServiceDescriptor.Singleton<ICommandDescriptorFactory, CommandDescriptorFactory>(),
                ServiceDescriptor.Transient<ICommandPipelineFactory, CommandPipelineFactory>(),
                ServiceDescriptor.Singleton<IEventDescriptorFactory, EventDescriptorFactory>(),
                ServiceDescriptor.Transient<IEventPipelineFactory, EventPipelineFactory>(),
                ServiceDescriptor.Transient<IMediator, Mediator>(),
            });

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<ICommandMetadataProvider, CancelableCommandMetadataProvider>(),
                ServiceDescriptor.Singleton(typeof(ICommandPipelineBehavior<,>), typeof(CancelableCommandBehavior<,>)),
                ServiceDescriptor.Transient(typeof(ICommandPipelineBehavior<,>), typeof(PreCommandHandlerBehavior<,>)),
                ServiceDescriptor.Transient(typeof(ICommandPipelineBehavior<,>), typeof(PostCommandHandlerBehavior<,>)),
                ServiceDescriptor.Singleton<IEventMetadataProvider, CancelableEventMetadataProvider>(),
                ServiceDescriptor.Singleton(typeof(IEventPipelineBehavior<>), typeof(CancelableEventBehavior<>)),
                ServiceDescriptor.Transient(typeof(IEventPipelineBehavior<>), typeof(PreEventHandlerBehavior<>)),
                ServiceDescriptor.Transient(typeof(IEventPipelineBehavior<>), typeof(PostEventHandlerBehavior<>)),
            });

        return new MediatorBuilder(services);
    }
}