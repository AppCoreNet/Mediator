﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements cancellation support for the <see cref="IEventContext"/>.
/// </summary>
public class CancelableEventFeature : ICancelableEventFeature
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelableEventFeature"/> class.
    /// </summary>
    /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource"/>.</param>
    public CancelableEventFeature(CancellationTokenSource cancellationTokenSource)
    {
        Ensure.Arg.NotNull(cancellationTokenSource);
        _cancellationTokenSource = cancellationTokenSource;
    }

    /// <summary>
    /// Notifies the <see cref="CancellationToken"/>.
    /// </summary>
    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }
}