// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Represents a type which instantiates <see cref="RequestDescriptor"/> objects.
/// </summary>
public interface IRequestDescriptorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="RequestDescriptor"/> for the specified <paramref name="requestType"/>.
    /// </summary>
    /// <param name="requestType">The type of the request.</param>
    /// <returns>The <see cref="RequestDescriptor"/>.</returns>
    RequestDescriptor CreateDescriptor(Type requestType);
}