// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Feature for cancelable events.
/// </summary>
public interface ICancelableEventFeature
{
    /// <summary>
    /// Cancels the event.
    /// </summary>
    void Cancel();
}