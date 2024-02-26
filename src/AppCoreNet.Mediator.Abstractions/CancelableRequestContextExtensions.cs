// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="IRequestContext"/> type.
/// </summary>
public static class CancelableRequestContextExtensions
{
    /// <summary>
    /// Gets a value indicating whether the request is cancelable.
    /// </summary>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    /// <returns><c>true</c> if the request can be canceled; <c>false</c> otherwise.</returns>
    public static bool IsCancelable(this IRequestContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.HasFeature<ICancelableRequestFeature>();
    }

    /// <summary>
    /// Cancels the request.
    /// </summary>
    /// <param name="context">The <see cref="IRequestContext"/>.</param>
    public static void Cancel(this IRequestContext context)
    {
        Ensure.Arg.NotNull(context);
        var feature = context.GetFeature<ICancelableRequestFeature>();
        feature.Cancel();
    }
}