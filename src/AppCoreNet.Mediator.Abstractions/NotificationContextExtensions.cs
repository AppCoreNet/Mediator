// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="INotificationContext"/>.
/// </summary>
public static class NotificationContextExtensions
{
    /// <summary>
    /// Adds a feature to the <see cref="INotificationContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    /// <param name="feature">The feature that should be added.</param>
    /// <exception cref="InvalidOperationException">The notification context feature is already registered.</exception>
    public static void AddFeature<T>(this INotificationContext context, T feature)
    {
        Ensure.Arg.NotNull(context);
        Ensure.Arg.NotNull(feature);

        try
        {
            context.Features.Add(typeof(T), feature);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException($"Notification context feature {typeof(T).GetDisplayName()} already registered.");
        }
    }

    /// <summary>
    /// Gets the feature with the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    /// <param name="feature">The feature.</param>
    /// <returns><c>true</c> if the feature was found; <c>false</c> otherwise.</returns>
    public static bool TryGetFeature<T>(this INotificationContext context, out T? feature)
    {
        Ensure.Arg.NotNull(context);

        if (context.Features.TryGetValue(typeof(T), out object? tmp))
        {
            feature = (T)tmp;
            return true;
        }

        feature = default;
        return false;
    }

    /// <summary>
    /// Gets the feature with the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    /// <returns>The feature.</returns>
    /// <exception cref="InvalidOperationException">The notification context feature is not available.</exception>
    public static T GetFeature<T>(this INotificationContext context)
    {
        if (!TryGetFeature(context, out T? feature))
            throw new InvalidOperationException($"Notification context feature {typeof(T).GetDisplayName()} is not available.");

        return feature!;
    }

    /// <summary>
    /// Gets a value indicating whether a feature is available.
    /// </summary>
    /// <typeparam name="T">The type of the feature.</typeparam>
    /// <param name="context">The <see cref="INotificationContext"/>.</param>
    /// <returns><c>true</c> if the feature is available; <c>false</c> otherwise.</returns>
    public static bool HasFeature<T>(this INotificationContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.Features.ContainsKey(typeof(T));
    }
}