// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Enables cancellation of the request or notification type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class CancelableAttribute : Attribute
{
}