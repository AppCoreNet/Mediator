// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator;

/// <summary>
/// Provides notification pipeline logger extensions.
/// </summary>
internal static partial class NotificationLog
{
    [LoggerMessage(
        EventId = 0,
        EventName = nameof(NotificationProcessing),
        Level = LogLevel.Trace,
        Message = "Processing notification {notificationType} ...")]
    public static partial void NotificationProcessing(this ILogger logger, Type notificationType);

    [LoggerMessage(
        EventId = 1,
        EventName = nameof(NotificationProcessed),
        Level = LogLevel.Debug,
        Message = "Successfully processed notification {notificationType} in {elapsedTime} ms.")]
    public static partial void NotificationProcessed(this ILogger logger, Type notificationType, long elapsedTime);

    public static void NotificationProcessed(this ILogger logger, Type notificationType, TimeSpan elapsedTime)
        => NotificationProcessed(logger, notificationType, (long)elapsedTime.TotalMilliseconds);

    [LoggerMessage(
        EventId = 2,
        EventName = nameof(NotificationFailed),
        Level = LogLevel.Error,
        Message = "Failed to process notification {notificationType} after {elapsedTime} ms.")]
    public static partial void NotificationFailed(this ILogger logger, Type notificationType, long elapsedTime, Exception exception);

    public static void NotificationFailed(this ILogger logger, Type notificationType, TimeSpan elapsedTime, Exception exception)
        => NotificationFailed(logger, notificationType, (long)elapsedTime.TotalMilliseconds, exception);

    [LoggerMessage(
        EventId = 3,
        EventName = nameof(NotificationShortCircuited),
        Level = LogLevel.Debug,
        Message = "Processing of notification {notificationType} was short-circuited by behavior {pipelineBehaviorType} in {elapsedTime} ms.")]
    public static partial void NotificationShortCircuited(
        this ILogger logger,
        Type notificationType,
        Type pipelineBehaviorType,
        long elapsedTime);

    public static void NotificationShortCircuited(
        this ILogger logger,
        Type notificationType,
        Type pipelineBehaviorType,
        TimeSpan elapsedTime) =>
        NotificationShortCircuited(logger, notificationType, pipelineBehaviorType, (long)elapsedTime.TotalMilliseconds);

    [LoggerMessage(
        EventId = 4,
        EventName = nameof(InvokingNotificationBehavior),
        Level = LogLevel.Trace,
        Message = "Invoking behavior {pipelineBehaviorType} for notification {notificationType} ...")]
    public static partial void InvokingNotificationBehavior(this ILogger logger, Type notificationType, Type pipelineBehaviorType);

    [LoggerMessage(
        EventId = 5,
        EventName = nameof(InvokingPreNotificationHandler),
        Level = LogLevel.Trace,
        Message = "Invoking pre-handler {notificationHandlerType} for notification {notificationType} ...")]
    public static partial void InvokingPreNotificationHandler(this ILogger logger, Type notificationType, Type notificationHandlerType);

    [LoggerMessage(
        EventId = 6,
        EventName = nameof(InvokingPostNotificationHandler),
        Level = LogLevel.Trace,
        Message = "Invoking post-handler {notificationHandlerType} for notification {notificationType} ...")]
    public static partial void InvokingPostNotificationHandler(this ILogger logger, Type notificationType, Type notificationHandlerType);
}