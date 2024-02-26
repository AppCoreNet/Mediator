// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a command.
/// </summary>
/// <typeparam name="TResult">The result.</typeparam>
public interface ICommand<out TResult>
{
}

/// <summary>
/// Represents a command without a result.
/// </summary>
public interface ICommand : ICommand<VoidResult>
{
}