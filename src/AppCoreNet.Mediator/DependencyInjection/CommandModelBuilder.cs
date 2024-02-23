// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

internal sealed class CommandModelBuilder : ICommandModelBuilder
{
    public IServiceCollection Services { get; }

    public CommandModelBuilder(IServiceCollection services)
    {
        Services = services;
    }
}