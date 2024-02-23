// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCore.CommandModel.Metadata;

/// <summary>
/// Represents a type which instantiates <see cref="CommandDescriptor"/> objects.
/// </summary>
public interface ICommandDescriptorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="CommandDescriptor"/> for the specified <paramref name="commandType"/>.
    /// </summary>
    /// <param name="commandType">The type of the event.</param>
    /// <returns>The <see cref="CommandDescriptor"/>.</returns>
    CommandDescriptor CreateDescriptor(Type commandType);
}