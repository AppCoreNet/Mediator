// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides request pipeline logger extensions.
/// </summary>
internal static partial class RequestLog
{
    [LoggerMessage(
        EventId = 0,
        EventName = nameof(RequestProcessing),
        Level = LogLevel.Trace,
        Message = "Processing request {requestType} ...")]
    public static partial void RequestProcessing(this ILogger logger, Type requestType);

    [LoggerMessage(
        EventId = 1,
        EventName = nameof(RequestProcessed),
        Level = LogLevel.Debug,
        Message = "Successfully processed request {requestType} in {elapsedTime} ms.")]
    public static partial void RequestProcessed(this ILogger logger, Type requestType, long elapsedTime);

    public static void RequestProcessed(this ILogger logger, Type requestType, TimeSpan elapsedTime)
        => RequestProcessed(logger, requestType, (long)elapsedTime.TotalMilliseconds);

    [LoggerMessage(
        EventId = 2,
        EventName = nameof(RequestFailed),
        Level = LogLevel.Error,
        Message = "Failed to process request {requestType} after {elapsedTime} ms.")]
    public static partial void RequestFailed(this ILogger logger, Type requestType, long elapsedTime, Exception exception);

    public static void RequestFailed(this ILogger logger, Type requestType, TimeSpan elapsedTime, Exception exception)
        => RequestFailed(logger, requestType, (long)elapsedTime.TotalMilliseconds, exception);

    [LoggerMessage(
        EventId = 3,
        EventName = nameof(RequestShortCircuited),
        Level = LogLevel.Debug,
        Message = "Processing of request {requestType} was short-circuited by behavior {pipelineBehaviorType} in {elapsedTime} ms.")]
    public static partial void RequestShortCircuited(
        this ILogger logger,
        Type requestType,
        Type pipelineBehaviorType,
        long elapsedTime);

    public static void RequestShortCircuited(
        this ILogger logger,
        Type requestType,
        Type pipelineBehaviorType,
        TimeSpan elapsedTime) =>
        RequestShortCircuited(logger, requestType, pipelineBehaviorType, (long)elapsedTime.TotalMilliseconds);

    [LoggerMessage(
        EventId = 4,
        EventName = nameof(InvokingRequestBehavior),
        Level = LogLevel.Trace,
        Message = "Invoking behavior {pipelineBehaviorType} for request {requestType} ...")]
    public static partial void InvokingRequestBehavior(this ILogger logger, Type requestType, Type pipelineBehaviorType);

    [LoggerMessage(
        EventId = 5,
        EventName = nameof(InvokingPreRequestHandler),
        Level = LogLevel.Trace,
        Message = "Invoking pre-handler {requestHandlerType} for request {requestType} ...")]
    public static partial void InvokingPreRequestHandler(this ILogger logger, Type requestType, Type requestHandlerType);

    [LoggerMessage(
        EventId = 6,
        EventName = nameof(InvokingPostRequestHandler),
        Level = LogLevel.Trace,
        Message = "Invoking post-handler {requestHandlerType} for request {requestType} ...")]
    public static partial void InvokingPostRequestHandler(this ILogger logger, Type requestType, Type requestHandlerType);
}