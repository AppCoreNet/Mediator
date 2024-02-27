// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Implements cancellation support for the <see cref="INotificationContext"/>.
/// </summary>
public class CancelableNotificationFeature : ICancelableNotificationFeature
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelableNotificationFeature"/> class.
    /// </summary>
    /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource"/>.</param>
    public CancelableNotificationFeature(CancellationTokenSource cancellationTokenSource)
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