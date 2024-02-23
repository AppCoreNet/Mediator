// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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