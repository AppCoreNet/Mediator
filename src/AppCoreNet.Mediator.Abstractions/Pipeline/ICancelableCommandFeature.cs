﻿// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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