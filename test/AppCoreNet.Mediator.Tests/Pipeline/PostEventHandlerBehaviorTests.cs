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

public class PostEventHandlerBehaviorTests
{
    [Fact]
    public async Task InvokesHandlersAfterNext()
    {
        var invokeOrder = new List<object>();

        var handlers = new[]
        {
            Substitute.For<IPostEventHandler<TestEvent>>(),
            Substitute.For<IPostEventHandler<TestEvent>>(),
        };

        handlers[0]
            .When(h => h.OnHandledAsync(Arg.Any<IEventContext<TestEvent>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[0]));

        handlers[1]
            .When(h => h.OnHandledAsync(Arg.Any<IEventContext<TestEvent>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[1]));

        var next = Substitute.For<EventPipelineDelegate<TestEvent>>();
        next.When(n => n.Invoke(Arg.Any<IEventContext<TestEvent>>(), Arg.Any<CancellationToken>()))
            .Do(_ => { invokeOrder.Add(next); });

        var @event = new TestEvent();
        var context = new EventContext<TestEvent>(
            new EventDescriptor(typeof(TestEvent), new Dictionary<string, object>()),
            @event);

        var behavior = new PostEventHandlerBehavior<TestEvent>(handlers, Substitute.For<ILogger<PostEventHandlerBehavior<TestEvent>>>());
        await behavior.HandleAsync(context, next);

        await next.Received(1)
                  .Invoke(
                      Arg.Is<IEventContext<TestEvent>>(e => e.Event.Equals(@event)),
                      Arg.Any<CancellationToken>());

        invokeOrder[0]
            .Should()
            .Be(next);

        await handlers[0]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<IEventContext<TestEvent>>(c => c.Event.Equals(@event)),
                  Arg.Any<CancellationToken>());

        invokeOrder[1]
            .Should()
            .Be(handlers[0]);

        await handlers[1]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<IEventContext<TestEvent>>(c => c.Event.Equals(@event)),
                  Arg.Any<CancellationToken>());

        invokeOrder[2]
            .Should()
            .Be(handlers[1]);
    }
}