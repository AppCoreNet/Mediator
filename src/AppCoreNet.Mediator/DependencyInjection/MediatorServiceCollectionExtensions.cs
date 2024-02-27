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
                ServiceDescriptor.Singleton<IRequestDescriptorFactory, RequestDescriptorFactory>(),
                ServiceDescriptor.Transient<IRequestPipelineFactory, RequestPipelineFactory>(),
                ServiceDescriptor.Singleton<INotificationDescriptorFactory, NotificationDescriptorFactory>(),
                ServiceDescriptor.Transient<INotificationPipelineFactory, NotificationPipelineFactory>(),
                ServiceDescriptor.Transient<IMediator, Mediator>(),
            });

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<IRequestMetadataProvider, CancelableRequestMetadataProvider>(),
                ServiceDescriptor.Singleton(typeof(IRequestPipelineBehavior<,>), typeof(CancelableRequestBehavior<,>)),
                ServiceDescriptor.Transient(typeof(IRequestPipelineBehavior<,>), typeof(PreRequestHandlerBehavior<,>)),
                ServiceDescriptor.Transient(typeof(IRequestPipelineBehavior<,>), typeof(PostRequestHandlerBehavior<,>)),
                ServiceDescriptor.Singleton<INotificationMetadataProvider, CancelableNotificationMetadataProvider>(),
                ServiceDescriptor.Singleton(typeof(INotificationPipelineBehavior<>), typeof(CancelableNotificationBehavior<>)),
                ServiceDescriptor.Transient(typeof(INotificationPipelineBehavior<>), typeof(PreNotificationHandlerBehavior<>)),
                ServiceDescriptor.Transient(typeof(INotificationPipelineBehavior<>), typeof(PostNotificationHandlerBehavior<>)),
            });

        return new MediatorBuilder(services);
    }
}