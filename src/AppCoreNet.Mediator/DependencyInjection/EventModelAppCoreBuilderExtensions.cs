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
/// Provides extension methods to configure the <see cref="IEventModelBuilder"/>.
/// </summary>
public static class EventModelAppCoreBuilderExtensions
{
    /// <summary>
    /// Adds the event model to the DI container.
    /// </summary>
    /// <param name="builder">The <see cref="IAppCoreBuilder"/>.</param>
    /// <param name="configure">The configure delegate.</param>
    /// <returns></returns>
    public static IEventModelBuilder AddEventModel(this IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);

        services.TryAdd(
            new[]
            {
                ServiceDescriptor.Transient<IEventPipelineResolver, EventPipelineResolver>(),
                ServiceDescriptor.Transient(typeof(IEventPipeline<>), typeof(EventPipeline<>)),
                ServiceDescriptor.Singleton<IEventContextFactory, EventContextFactory>(),
                ServiceDescriptor.Singleton<IEventDescriptorFactory, EventDescriptorFactory>(),
                ServiceDescriptor.Transient<IEventPublisher, EventPublisher>()
            });

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<IEventMetadataProvider, CancelableEventMetadataProvider>(),
                ServiceDescriptor.Singleton<IEventMetadataProvider, TopicMetadataProvider>(),
                ServiceDescriptor.Singleton(typeof(IEventPipelineBehavior<>), typeof(CancelableEventBehavior<>)),
                ServiceDescriptor.Transient(
                    typeof(IEventPipelineBehavior<>),
                    typeof(PreEventHandlerBehavior<>)),
                ServiceDescriptor.Transient(
                    typeof(IEventPipelineBehavior<>),
                    typeof(PostEventHandlerBehavior<>))
            });

        return new EventModelBuilder(services);
    }
}