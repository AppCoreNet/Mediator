// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Threading;
using AppCoreNet.Diagnostics;

namespace AppCore.CommandModel.Pipeline;

/// <summary>
/// Implements cancellation support for the <see cref="ICommandContext"/>.
/// </summary>
public class CancelableCommandFeature : ICancelableCommandFeature
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelableCommandFeature"/> class.
    /// </summary>
    /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource"/>.</param>
    public CancelableCommandFeature(CancellationTokenSource cancellationTokenSource)
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