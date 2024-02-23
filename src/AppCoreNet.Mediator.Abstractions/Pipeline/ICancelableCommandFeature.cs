// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Feature for cancelable commands.
/// </summary>
public interface ICancelableCommandFeature
{
    /// <summary>
    /// Cancels the command.
    /// </summary>
    void Cancel();
}