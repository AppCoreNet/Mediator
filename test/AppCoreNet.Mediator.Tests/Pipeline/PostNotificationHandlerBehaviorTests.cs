// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class PostNotificationHandlerBehaviorTests
{
    [Fact]
    public async Task InvokesHandlersAfterNext()
    {
        var invokeOrder = new List<object>();

        var handlers = new[]
        {
            Substitute.For<IPostNotificationHandler<TestNotification>>(),
            Substitute.For<IPostNotificationHandler<TestNotification>>(),
        };

        handlers[0]
            .When(h => h.OnHandledAsync(Arg.Any<INotificationContext<TestNotification>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[0]));

        handlers[1]
            .When(h => h.OnHandledAsync(Arg.Any<INotificationContext<TestNotification>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[1]));

        var next = Substitute.For<NotificationPipelineDelegate<TestNotification>>();
        next.When(n => n.Invoke(Arg.Any<INotificationContext<TestNotification>>(), Arg.Any<CancellationToken>()))
            .Do(_ => { invokeOrder.Add(next); });

        var notification = new TestNotification();
        var context = new NotificationContext<TestNotification>(
            new NotificationDescriptor(typeof(TestNotification), new Dictionary<string, object>()),
            notification);

        var behavior = new PostNotificationHandlerBehavior<TestNotification>(handlers, Substitute.For<ILogger<PostNotificationHandlerBehavior<TestNotification>>>());
        await behavior.HandleAsync(context, next);

        await next.Received(1)
                  .Invoke(
                      Arg.Is<INotificationContext<TestNotification>>(e => e.Notification.Equals(notification)),
                      Arg.Any<CancellationToken>());

        invokeOrder[0]
            .Should()
            .Be(next);

        await handlers[0]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<INotificationContext<TestNotification>>(c => c.Notification.Equals(notification)),
                  Arg.Any<CancellationToken>());

        invokeOrder[1]
            .Should()
            .Be(handlers[0]);

        await handlers[1]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<INotificationContext<TestNotification>>(c => c.Notification.Equals(notification)),
                  Arg.Any<CancellationToken>());

        invokeOrder[2]
            .Should()
            .Be(handlers[1]);
    }
}