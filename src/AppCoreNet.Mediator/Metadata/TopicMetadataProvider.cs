// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Provides metadata for event topic.
/// </summary>
public class TopicMetadataProvider : IEventMetadataProvider
{
    /// <inheritdoc />
    public void GetMetadata(Type eventType, IDictionary<string, object> metadata)
    {
        TypeInfo eventTypeInfo = eventType.GetTypeInfo();

        var topicAttribute = eventTypeInfo.GetCustomAttribute<TopicAttribute>();
        string? topic = topicAttribute?.Name;
        if (!string.IsNullOrEmpty(topic))
        {
            metadata.Add(EventMetadataKeys.TopicMetadataKey, topic!);
        }
    }
}