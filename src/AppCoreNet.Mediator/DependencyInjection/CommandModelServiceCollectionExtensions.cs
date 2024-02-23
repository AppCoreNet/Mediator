// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.CommandModel;
using AppCore.CommandModel.Metadata;
using AppCore.CommandModel.Pipeline;
using AppCoreNet.Diagnostics;
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