// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="ICommandContext"/>.
/// </summary>
public static class CommandContextExtensions
{
    /// <summary>
    /// Adds a feature to the <see cref="ICommandContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    /// <param name="feature">The feature that should be added.</param>
    /// <exception cref="InvalidOperationException">The command context feature is already registered.</exception>
    public static void AddFeature<T>(this ICommandContext context, T feature)
    {
        Ensure.Arg.NotNull(context);
        Ensure.Arg.NotNull(feature);

        try
        {
            context.Features.Add(typeof(T), feature);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException($"Command context feature {typeof(T).GetDisplayName()} already registered.");
        }
    }

    /// <summary>
    /// Gets the feature with the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    /// <returns>The feature.</returns>
    /// <exception cref="InvalidOperationException">The command context feature is not available.</exception>
    public static T GetFeature<T>(this ICommandContext context)
    {
        Ensure.Arg.NotNull(context);

        if (!context.Features.TryGetValue(typeof(T), out object feature))
            throw new InvalidOperationException($"Command context feature {typeof(T).GetDisplayName()} is not available.");

        return (T) feature;
    }

    /// <summary>
    /// Gets a value indicating whether a feature is available.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    /// <returns><c>true</c> if the feature is available; <c>false</c> otherwise.</returns>
    public static bool HasFeature<T>(this ICommandContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.Features.ContainsKey(typeof(T));
    }
}