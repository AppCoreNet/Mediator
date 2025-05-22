// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides extension methods for the <see cref="IRequestContext"/>.
/// </summary>
public static class RequestContextExtensions
{
    /// <summary>
    /// Adds a feature to the <see cref="IRequestContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    /// <param name="feature">The feature that should be added.</param>
    /// <exception cref="InvalidOperationException">The feature is already registered.</exception>
    public static void AddFeature<T>(this IRequestContext context, T feature)
    {
        Ensure.Arg.NotNull(context);
        Ensure.Arg.NotNull(feature);

        try
        {
            context.Features.Add(typeof(T), feature);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException($"Request context feature {typeof(T).GetDisplayName()} already registered.");
        }
    }

    /// <summary>
    /// Gets the feature with the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    /// <returns>The feature.</returns>
    /// <exception cref="InvalidOperationException">The feature is not available.</exception>
    public static T GetFeature<T>(this IRequestContext context)
    {
        Ensure.Arg.NotNull(context);

        if (!context.Features.TryGetValue(typeof(T), out object? feature))
            throw new InvalidOperationException($"Request context feature {typeof(T).GetDisplayName()} is not available.");

        return (T)feature;
    }

    /// <summary>
    /// Gets a value indicating whether a feature is available.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    /// <returns><c>true</c> if the feature is available; <c>false</c> otherwise.</returns>
    public static bool HasFeature<T>(this IRequestContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.Features.ContainsKey(typeof(T));
    }
}