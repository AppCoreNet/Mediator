// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.CommandModel;

/// <summary>
/// Represents a command.
/// </summary>
public interface ICommand<out TResult>
{
}

/// <summary>
/// Represents a command without a result.
/// </summary>
public interface ICommand : ICommand<VoidResult>
{
}