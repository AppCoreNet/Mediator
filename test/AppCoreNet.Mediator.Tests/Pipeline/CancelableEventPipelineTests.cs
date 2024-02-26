// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class CancelableEventPipelineTests
{
    [Fact]
    public async Task CancelThrowsOperationCanceledException()
    {
        var handler = Substitute.For<IEventHandler<CancelableTestEvent>>();

        var behavior = Substitute.For<IEventPipelineBehavior<CancelableTestEvent>>();
        behavior.When(
                    h => h.HandleAsync(
                        Arg.Any<IEventContext<CancelableTestEvent>>(),
                        Arg.Any<EventPipelineDelegate<CancelableTestEvent>>(),
                        Arg.Any<CancellationToken>()))
                .Do(
                    ci => ci.ArgAt<IEventContext<CancelableTestEvent>>(0)
                            .Cancel());

        var metadata = new Dictionary<string, object>();
        new CancelableEventMetadataProvider().GetMetadata(typeof(CancelableTestEvent), metadata);

        var @event = new CancelableTestEvent();
        Type eventType = typeof(CancelableTestEvent);

        var eventDescriptor = new EventDescriptor(eventType, metadata);

        var descriptorFactory = Substitute.For<IEventDescriptorFactory>();
        descriptorFactory.CreateDescriptor(eventType)
                         .Returns(eventDescriptor);

        var pipeline = new EventPipeline<CancelableTestEvent>(
            descriptorFactory,
            new[] { new CancelableEventBehavior<CancelableTestEvent>(), behavior },
            new[] { handler },
            Substitute.For<ILogger<EventPipeline<CancelableTestEvent>>>());

        Func<Task> invoke = async () => { await pipeline.InvokeAsync(@event); };

        await invoke.Should()
                    .ThrowAsync<OperationCanceledException>();
    }
}