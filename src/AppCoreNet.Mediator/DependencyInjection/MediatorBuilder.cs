// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

internal sealed class MediatorBuilder : IMediatorBuilder
{
    public IServiceCollection Services { get; }

    public MediatorBuilder(IServiceCollection services)
    {
        Services = services;
    }
}