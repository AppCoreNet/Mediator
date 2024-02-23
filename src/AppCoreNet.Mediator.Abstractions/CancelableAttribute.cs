// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Enables cancellation for the event type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class CancelableAttribute : Attribute
{
}