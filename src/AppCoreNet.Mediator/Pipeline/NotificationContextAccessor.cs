// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Default implementation of the <see cref="INotificationContextAccessor"/> interface.
/// </summary>
public sealed class NotificationContextAccessor : INotificationContextAccessor
{
    private readonly AsyncLocal<INotificationContext?> _context = new ();

    /// <inheritdoc />
    public INotificationContext? CurrentContext
    {
        get => _context.Value;
        set => _context.Value = value;
    }
}