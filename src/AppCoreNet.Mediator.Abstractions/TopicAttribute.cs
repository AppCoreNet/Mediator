// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Mediator;

/// <summary>
/// Decorates an event with a topic.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TopicAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the event topic.
    /// </summary>
    public string? Name { get; set; }
}