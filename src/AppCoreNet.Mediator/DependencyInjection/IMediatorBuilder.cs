// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides a builder object for the mediator.
/// </summary>
public interface IMediatorBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    IServiceCollection Services { get; }
}