// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// When applied to a command type, requires authorization of the user.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute : Attribute
{
}