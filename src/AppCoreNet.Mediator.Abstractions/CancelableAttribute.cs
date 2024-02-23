// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Enables cancellation for the event type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class CancelableAttribute : Attribute
{
}