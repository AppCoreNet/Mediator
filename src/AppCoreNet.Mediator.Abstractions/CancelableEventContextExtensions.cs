// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="IEventContext"/> type.
/// </summary>
public static class CancelableEventContextExtensions
{
    /// <summary>
    /// Gets a value indicating whether the event is cancelable.
    /// </summary>
    /// <param name="context">The <see cref="IEventContext"/>.</param>
    /// <returns><c>true</c> if the event can be canceled; <c>false</c> otherwise.</returns>
    public static bool IsCancelable(this IEventContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.HasFeature<ICancelableEventFeature>();
    }

    /// <summary>
    /// Cancels the event.
    /// </summary>
    /// <param name="context">The <see cref="IEventContext"/>.</param>
    public static void Cancel(this IEventContext context)
    {
        Ensure.Arg.NotNull(context);
        var feature = context.GetFeature<ICancelableEventFeature>();
        feature.Cancel();
    }
}