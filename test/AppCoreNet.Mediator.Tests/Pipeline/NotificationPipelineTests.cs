// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class NotificationPipelineTests
{
    [Fact]
    public async Task InvokesHandlers()
    {
        var handler1 = Substitute.For<INotificationHandler<TestNotification>>();
        var handler2 = Substitute.For<INotificationHandler<TestNotification>>();

        var notification = new TestNotification();
        Type notificationType = typeof(TestNotification);

        var descriptor = new NotificationDescriptor(notificationType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<INotificationDescriptorFactory>();
        descriptorFactory.CreateDescriptor(notificationType)
                         .Returns(descriptor);

        var pipeline = new NotificationPipeline<TestNotification>(
            descriptorFactory,
            Enumerable.Empty<INotificationPipelineBehavior<TestNotification>>(),
            new[] { handler1, handler2 },
            Substitute.For<ILogger<NotificationPipeline<TestNotification>>>());

        await pipeline.InvokeAsync(notification);

        await handler1.Received(1)
                      .HandleAsync(
                          Arg.Is(notification),
                          Arg.Any<CancellationToken>());

        await handler2.Received(1)
                      .HandleAsync(
                          Arg.Is(notification),
                          Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokesBehaviors()
    {
        var invokedBehaviors = new List<INotificationPipelineBehavior<TestNotification>>();

        var behavior1 = Substitute.For<INotificationPipelineBehavior<TestNotification>>();
        behavior1.When(
                     b => b.HandleAsync(
                         Arg.Any<INotificationContext<TestNotification>>(),
                         Arg.Any<NotificationPipelineDelegate<TestNotification>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior1);
                         await ci.ArgAt<NotificationPipelineDelegate<TestNotification>>(1)(
                             ci.ArgAt<INotificationContext<TestNotification>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var behavior2 = Substitute.For<INotificationPipelineBehavior<TestNotification>>();
        behavior2.When(
                     b => b.HandleAsync(
                         Arg.Any<INotificationContext<TestNotification>>(),
                         Arg.Any<NotificationPipelineDelegate<TestNotification>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior2);
                         await ci.ArgAt<NotificationPipelineDelegate<TestNotification>>(1)(
                             ci.ArgAt<INotificationContext<TestNotification>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var notification = new TestNotification();
        Type notificationType = typeof(TestNotification);

        var descriptor = new NotificationDescriptor(notificationType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<INotificationDescriptorFactory>();
        descriptorFactory.CreateDescriptor(notificationType)
                         .Returns(descriptor);

        var pipeline = new NotificationPipeline<TestNotification>(
            descriptorFactory,
            new[] { behavior1, behavior2 },
            Enumerable.Empty<INotificationHandler<TestNotification>>(),
            Substitute.For<ILogger<NotificationPipeline<TestNotification>>>());

        await pipeline.InvokeAsync(notification);

        await behavior1.Received(1)
                       .HandleAsync(
                           Arg.Is<INotificationContext<TestNotification>>(
                               i => i.Notification == notification && i.NotificationDescriptor == descriptor),
                           Arg.Any<NotificationPipelineDelegate<TestNotification>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[0]
            .Should()
            .Be(behavior1);

        await behavior2.Received(1)
                       .HandleAsync(
                           Arg.Is<INotificationContext<TestNotification>>(
                               i => i.Notification == notification && i.NotificationDescriptor == descriptor),
                           Arg.Any<NotificationPipelineDelegate<TestNotification>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[1]
            .Should()
            .Be(behavior2);
    }

    [Fact]
    public async Task AssignsCurrentContext()
    {
        var handler = Substitute.For<INotificationHandler<TestNotification>>();
        var accessor = Substitute.For<INotificationContextAccessor>();

        var notification = new TestNotification();
        Type notificationType = typeof(TestNotification);

        var descriptor = new NotificationDescriptor(notificationType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<INotificationDescriptorFactory>();
        descriptorFactory.CreateDescriptor(notificationType)
                         .Returns(descriptor);

        var pipeline = new NotificationPipeline<TestNotification>(
            descriptorFactory,
            Enumerable.Empty<INotificationPipelineBehavior<TestNotification>>(),
            new[] { handler },
            Substitute.For<ILogger<NotificationPipeline<TestNotification>>>(),
            accessor);

        await pipeline.InvokeAsync(notification);

        accessor.Received(1)
                .CurrentContext =
            Arg.Is<INotificationContext<TestNotification>>(c => c.Notification == notification && c.NotificationDescriptor == descriptor);

        accessor.Received(1)
                .CurrentContext = null;
    }
}