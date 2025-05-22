// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Default implementation of the <see cref="IRequestContextAccessor"/> interface.
/// </summary>
public sealed class RequestContextAccessor : IRequestContextAccessor
{
    private readonly AsyncLocal<IRequestContext?> _context = new();

    /// <inheritdoc />
    public IRequestContext? CurrentContext
    {
        get => _context.Value;
        set => _context.Value = value;
    }
}