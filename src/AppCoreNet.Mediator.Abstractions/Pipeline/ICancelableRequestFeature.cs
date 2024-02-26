// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Feature for cancelable request.
/// </summary>
public interface ICancelableRequestFeature
{
    /// <summary>
    /// Cancels the request.
    /// </summary>
    void Cancel();
}