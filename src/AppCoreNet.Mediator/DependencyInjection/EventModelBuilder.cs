// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

internal sealed class EventModelBuilder : IEventModelBuilder
{
    public IServiceCollection Services { get; }

    public EventModelBuilder(IServiceCollection services)
    {
        Services = services;
    }
}