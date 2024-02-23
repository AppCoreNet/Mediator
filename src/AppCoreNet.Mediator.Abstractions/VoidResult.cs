// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a <see cref="Void"/> command result.
/// </summary>
/// <seealso cref="ICommand"/>
public sealed class VoidResult : IEquatable<VoidResult>
{
    /// <summary>
    /// Gets the singleton instance of <see cref="VoidResult"/>.
    /// </summary>
    public static readonly VoidResult Instance = new VoidResult();

    private VoidResult()
    {
    }

    /// <inheritdoc />
    public bool Equals(VoidResult other)
    {
        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is VoidResult other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 735275808;
    }

    /// <summary>
    /// Compares two instances of <see cref="VoidResult"/> for equality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(VoidResult left, VoidResult right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Compares two instances of <see cref="VoidResult"/> for un-equality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(VoidResult left, VoidResult right)
    {
        return !Equals(left, right);
    }
}