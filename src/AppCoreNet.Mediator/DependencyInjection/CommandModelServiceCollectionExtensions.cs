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
/// Provides extension methods to configure the <see cref="ICommandModelBuilder"/>.
/// </summary>
public static class CommandModelServiceCollectionExtensions
{
    /// <summary>
    /// Adds the command model to the DI container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The passed <see cref="ICommandModelBuilder"/>.</returns>
    public static ICommandModelBuilder AddCommandModel(this IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);

        services.TryAdd(
            new[]
            {
                ServiceDescriptor.Transient<ICommandProcessor, CommandProcessor>(),
                ServiceDescriptor.Singleton<ICommandDescriptorFactory, CommandDescriptorFactory>()
            });

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Singleton<ICommandMetadataProvider, CancelableCommandMetadataProvider>(),
                ServiceDescriptor.Singleton(
                    typeof(ICommandPipelineBehavior<,>),
                    typeof(CancelableCommandBehavior<,>)),
                ServiceDescriptor.Transient(
                    typeof(ICommandPipelineBehavior<,>),
                    typeof(PreCommandHandlerBehavior<,>)),
                ServiceDescriptor.Transient(
                    typeof(ICommandPipelineBehavior<,>),
                    typeof(PostCommandHandlerBehavior<,>)),
            });

        return new CommandModelBuilder(services);
    }
}