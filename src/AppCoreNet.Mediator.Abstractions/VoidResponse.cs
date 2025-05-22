// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a <see cref="Void"/> request response.
/// </summary>
/// <seealso cref="IRequest"/>
public sealed class VoidResponse : IEquatable<VoidResponse>
{
    /// <summary>
    /// Gets the singleton instance of <see cref="VoidResponse"/>.
    /// </summary>
    public static readonly VoidResponse Instance = new();

    private VoidResponse()
    {
    }

    /// <inheritdoc />
    public bool Equals(VoidResponse? other)
    {
        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is VoidResponse other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 735275808;
    }

    /// <summary>
    /// Compares two instances of <see cref="VoidResponse"/> for equality.
    /// </summary>
    /// <param name="left">The first <see cref="VoidResponse"/>.</param>
    /// <param name="right">The second <see cref="VoidResponse"/>.</param>
    /// <returns><c>true</c> if both instances are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(VoidResponse left, VoidResponse right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Compares two instances of <see cref="VoidResponse"/> for in-equality.
    /// </summary>
    /// <param name="left">The first <see cref="VoidResponse"/>.</param>
    /// <param name="right">The second <see cref="VoidResponse"/>.</param>
    /// <returns><c>true</c> if both instances are not equal; <c>false</c> otherwise.</returns>
    public static bool operator !=(VoidResponse left, VoidResponse right)
    {
        return !Equals(left, right);
    }
}