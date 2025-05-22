﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides extension methods for the <see cref="NotificationDescriptor"/> type.
/// </summary>
public static class NotificationDescriptorExtensions
{
    /// <summary>
    /// Tries to get an metadata entry with the specified key.
    /// </summary>
    /// <param name="descriptor">The <see cref="NotificationDescriptor"/>.</param>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns><c>true</c> if the metadata entry was found; <c>false</c> otherwise.</returns>
    /// <exception cref="InvalidCastException">The metadata value is not of the specified type.</exception>
    public static bool TryGetMetadata<T>(this NotificationDescriptor descriptor, string key, out T? value)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.NotEmpty(key);

        if (!descriptor.Metadata.TryGetValue(key, out object? tmp))
        {
            value = default;
            return false;
        }

        if (!(tmp is T))
        {
            throw new InvalidCastException(
                $"Metadata value with key {key} is not of the expected type {typeof(T).GetDisplayName()}");
        }

        value = (T)tmp;
        return true;
    }

    /// <summary>
    /// Gets a metadata entry with the specified key. Returns the <paramref name="defaultValue"/> if not found.
    /// </summary>
    /// <param name="descriptor">The <see cref="NotificationDescriptor"/>.</param>
    /// <param name="key">The metadata key.</param>
    /// <param name="defaultValue">The metadata default value.</param>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns>The metadata value if the entry was found; <paramref name="defaultValue"/> otherwise.</returns>
    /// <exception cref="InvalidCastException">The metadata value is not of the specified type.</exception>
    public static T? GetMetadata<T>(this NotificationDescriptor descriptor, string key, T? defaultValue)
    {
        if (!TryGetMetadata(descriptor, key, out T? result))
            result = defaultValue;

        return result;
    }

    /// <summary>
    /// Gets a metadata entry with the specified key.
    /// </summary>
    /// <param name="descriptor">The <see cref="NotificationDescriptor"/>.</param>
    /// <param name="key">The metadata key.</param>
    /// <typeparam name="T">The type of the metadata value.</typeparam>
    /// <returns>The metadata value.</returns>
    /// <exception cref="InvalidCastException">The metadata value is not of the specified type.</exception>
    /// <exception cref="KeyNotFoundException">The metadata entry was not found.</exception>
    public static T GetMetadata<T>(this NotificationDescriptor descriptor, string key)
    {
        if (!TryGetMetadata(descriptor, key, out T? value))
            throw new KeyNotFoundException($"Metadata value with key {key} is unknown.");

        return value!;
    }
}