// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides access to the currently processed <see cref="IRequestContext"/>.
/// </summary>
public interface IRequestContextAccessor
{
    /// <summary>
    /// Gets or sets the current <see cref="IRequestContext"/>.
    /// </summary>
    IRequestContext? CurrentContext { get; set; }
}