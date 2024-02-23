// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Pipeline;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides extension methods for the <see cref="ICommandContext"/> type.
/// </summary>
public static class CancelableCommandContextExtensions
{
    /// <summary>
    /// Gets a value indicating whether the command is cancelable.
    /// </summary>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    /// <returns><c>true</c> if the command can be canceled; <c>false</c> otherwise.</returns>
    public static bool IsCancelable(this ICommandContext context)
    {
        Ensure.Arg.NotNull(context);
        return context.HasFeature<ICancelableCommandFeature>();
    }

    /// <summary>
    /// Cancels the command.
    /// </summary>
    /// <param name="context">The <see cref="ICommandContext"/>.</param>
    public static void Cancel(this ICommandContext context)
    {
        Ensure.Arg.NotNull(context);
        var feature = context.GetFeature<ICancelableCommandFeature>();
        feature.Cancel();
    }
}