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

public class EventPipelineTests
{
    [Fact]
    public async Task InvokesHandlers()
    {
        var handler1 = Substitute.For<IEventHandler<TestEvent>>();
        var handler2 = Substitute.For<IEventHandler<TestEvent>>();

        var @event = new TestEvent();
        Type eventType = typeof(TestEvent);

        var eventDescriptor = new EventDescriptor(eventType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IEventDescriptorFactory>();
        descriptorFactory.CreateDescriptor(eventType)
                         .Returns(eventDescriptor);

        var pipeline = new EventPipeline<TestEvent>(
            descriptorFactory,
            Enumerable.Empty<IEventPipelineBehavior<TestEvent>>(),
            new[] { handler1, handler2 },
            Substitute.For<ILogger<EventPipeline<TestEvent>>>());

        await pipeline.InvokeAsync(@event);

        await handler1.Received(1)
                      .HandleAsync(
                          Arg.Is(@event),
                          Arg.Any<CancellationToken>());

        await handler2.Received(1)
                      .HandleAsync(
                          Arg.Is(@event),
                          Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokesBehaviors()
    {
        var invokedBehaviors = new List<IEventPipelineBehavior<TestEvent>>();

        var behavior1 = Substitute.For<IEventPipelineBehavior<TestEvent>>();
        behavior1.When(
                     b => b.HandleAsync(
                         Arg.Any<IEventContext<TestEvent>>(),
                         Arg.Any<EventPipelineDelegate<TestEvent>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior1);
                         await ci.ArgAt<EventPipelineDelegate<TestEvent>>(1)(
                             ci.ArgAt<IEventContext<TestEvent>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var behavior2 = Substitute.For<IEventPipelineBehavior<TestEvent>>();
        behavior2.When(
                     b => b.HandleAsync(
                         Arg.Any<IEventContext<TestEvent>>(),
                         Arg.Any<EventPipelineDelegate<TestEvent>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior2);
                         await ci.ArgAt<EventPipelineDelegate<TestEvent>>(1)(
                             ci.ArgAt<IEventContext<TestEvent>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var @event = new TestEvent();
        Type eventType = typeof(TestEvent);

        var eventDescriptor = new EventDescriptor(eventType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IEventDescriptorFactory>();
        descriptorFactory.CreateDescriptor(eventType)
                         .Returns(eventDescriptor);

        var pipeline = new EventPipeline<TestEvent>(
            descriptorFactory,
            new[] { behavior1, behavior2 },
            Enumerable.Empty<IEventHandler<TestEvent>>(),
            Substitute.For<ILogger<EventPipeline<TestEvent>>>());

        await pipeline.InvokeAsync(@event);

        await behavior1.Received(1)
                       .HandleAsync(
                           Arg.Is<IEventContext<TestEvent>>(
                               i => i.Event == @event && i.EventDescriptor == eventDescriptor),
                           Arg.Any<EventPipelineDelegate<TestEvent>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[0]
            .Should()
            .Be(behavior1);

        await behavior2.Received(1)
                       .HandleAsync(
                           Arg.Is<IEventContext<TestEvent>>(
                               i => i.Event == @event && i.EventDescriptor == eventDescriptor),
                           Arg.Any<EventPipelineDelegate<TestEvent>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[1]
            .Should()
            .Be(behavior2);
    }

    [Fact]
    public async Task AssignsEventContext()
    {
        var handler = Substitute.For<IEventHandler<TestEvent>>();
        var accessor = Substitute.For<IEventContextAccessor>();

        var @event = new TestEvent();
        Type eventType = typeof(TestEvent);

        var eventDescriptor = new EventDescriptor(eventType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IEventDescriptorFactory>();
        descriptorFactory.CreateDescriptor(eventType)
                         .Returns(eventDescriptor);

        var pipeline = new EventPipeline<TestEvent>(
            descriptorFactory,
            Enumerable.Empty<IEventPipelineBehavior<TestEvent>>(),
            new[] { handler },
            Substitute.For<ILogger<EventPipeline<TestEvent>>>(),
            accessor);

        await pipeline.InvokeAsync(@event);

        accessor.Received(1)
                .EventContext =
            Arg.Is<IEventContext<TestEvent>>(c => c.Event == @event && c.EventDescriptor == eventDescriptor);

        accessor.Received(1)
                .EventContext = null;
    }
}