﻿// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides a builder object for command model services.
/// </summary>
public interface ICommandModelBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    IServiceCollection Services { get; }
}