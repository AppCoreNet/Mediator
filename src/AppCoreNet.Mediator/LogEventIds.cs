// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator;

internal class LogEventIds
{
    // EventPipeline
    public static readonly EventId PipelineProcessing = new EventId(0, nameof(PipelineProcessing));

    public static readonly EventId PipelineProcessed = new EventId(1, nameof(PipelineProcessed));

    public static readonly EventId PipelineFailed = new EventId(2, nameof(PipelineFailed));

    public static readonly EventId PipelineShortCircuited = new EventId(3, nameof(PipelineShortCircuited));

    public static readonly EventId InvokingBehavior = new EventId(4, nameof(InvokingBehavior));

    public static readonly EventId InvokingPreEventHandler = new EventId(5, nameof(InvokingPreEventHandler));

    public static readonly EventId InvokingPostEventHandler = new EventId(6, nameof(InvokingPostEventHandler));
}