// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator;

internal static class LoggerExtensions
{
    // EventPipeline
    private static readonly Action<ILogger, string, Exception?> _pipelineProcessing =
        LoggerMessage.Define<string>(
            LogLevel.Trace,
            LogEventIds.PipelineProcessing,
            "Processing event {eventType} ...");

    private static readonly Action<ILogger, string, long, Exception?> _pipelineProcessed =
        LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            LogEventIds.PipelineProcessed,
            "Successfully processed event {eventType} in {elapsedTime} ms.");

    private static readonly Action<ILogger, string, long, Exception?> _pipelineFailed =
        LoggerMessage.Define<string, long>(
            LogLevel.Error,
            LogEventIds.PipelineFailed,
            "Failed to process event {eventType} after {elapsedTime} ms.");

    private static readonly Action<ILogger, string, string, long, Exception?> _pipelineShortCircuited =
        LoggerMessage.Define<string, string, long>(
            LogLevel.Debug,
            LogEventIds.PipelineShortCircuited,
            "Processing of event {eventType} was short-circuited by behavior {eventPipelineBehaviorType} in {elapsedTime} ms.");

    private static readonly Action<ILogger, string, string, Exception?> _invokingBehavior =
        LoggerMessage.Define<string, string>(
            LogLevel.Trace,
            LogEventIds.InvokingBehavior,
            "Invoking behavior {pipelineBehaviorType} for event {eventType} ...");

    private static readonly Action<ILogger, string, string, Exception?> _invokingPreEventHandler =
        LoggerMessage.Define<string, string>(
            LogLevel.Trace,
            LogEventIds.InvokingPreEventHandler,
            "Invoking pre-handler {preEventHandlerType} for event {eventType} ...");

    private static readonly Action<ILogger, string, string, Exception?> _invokingPostEventHandler =
        LoggerMessage.Define<string, string>(
            LogLevel.Trace,
            LogEventIds.InvokingPostEventHandler,
            "Invoking post-handler {postEventHandlerType} for event {eventType} ...");

    public static void PipelineProcessing(this ILogger logger, Type eventType)
    {
        _pipelineProcessing(logger, eventType.GetDisplayName(), null);
    }

    public static void PipelineProcessed(this ILogger logger, Type eventType, TimeSpan elapsed)
    {
        _pipelineProcessed(logger, eventType.GetDisplayName(), (long) elapsed.TotalMilliseconds, null);
    }

    public static void PipelineFailed(this ILogger logger, Type eventType, TimeSpan elapsed, Exception exception)
    {
        _pipelineFailed(logger, eventType.GetDisplayName(), (long) elapsed.TotalMilliseconds, exception);
    }

    public static void PipelineShortCircuited(this ILogger logger, Type eventType, Type eventPipelineBehavior, TimeSpan elapsed)
    {
        _pipelineShortCircuited(
            logger,
            eventType.GetDisplayName(),
            eventPipelineBehavior.GetDisplayName(),
            (long) elapsed.TotalMilliseconds,
            null);
    }

    public static void InvokingBehavior(this ILogger logger, Type eventType, Type pipelineBehaviorType)
    {
        _invokingBehavior(logger, pipelineBehaviorType.GetDisplayName(), eventType.GetDisplayName(), null);
    }

    public static void InvokingPreEventHandler(this ILogger logger, Type eventType, Type preHandlerType)
    {
        _invokingPreEventHandler(logger, preHandlerType.GetDisplayName(), eventType.GetDisplayName(), null);
    }

    public static void InvokingPostEventHandler(this ILogger logger, Type eventType, Type postHandlerType)
    {
        _invokingPostEventHandler(logger, postHandlerType.GetDisplayName(), eventType.GetDisplayName(), null);
    }
}