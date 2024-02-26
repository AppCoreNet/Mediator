// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Default implementation of the <see cref="IEventContextAccessor"/> interface.
/// </summary>
public sealed class EventContextAccessor : IEventContextAccessor
{
    private readonly AsyncLocal<IEventContext?> _eventContext = new ();

    /// <inheritdoc />
    public IEventContext? EventContext
    {
        get => _eventContext.Value;
        set => _eventContext.Value = value;
    }
}