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

public class RequestPipelineTests
{
    [Fact]
    public async Task InvokesHandlers()
    {
        var handler1 = Substitute.For<IRequestHandler<TestRequest, TestResponse>>();

        var request = new TestRequest();
        Type requestType = typeof(TestRequest);

        var descriptor = new RequestDescriptor(requestType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IRequestDescriptorFactory>();
        descriptorFactory.CreateDescriptor(requestType)
                         .Returns(descriptor);

        var pipeline = new RequestPipeline<TestRequest, TestResponse>(
            descriptorFactory,
            Enumerable.Empty<IRequestPipelineBehavior<TestRequest, TestResponse>>(),
            handler1,
            Substitute.For<ILogger<RequestPipeline<TestRequest, TestResponse>>>());

        await pipeline.InvokeAsync(request);

        await handler1.Received(1)
                      .HandleAsync(
                          Arg.Is(request),
                          Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokesBehaviors()
    {
        var invokedBehaviors = new List<IRequestPipelineBehavior<TestRequest, TestResponse>>();

        var behavior1 = Substitute.For<IRequestPipelineBehavior<TestRequest, TestResponse>>();
        behavior1.When(
                     b => b.HandleAsync(
                         Arg.Any<IRequestContext<TestRequest, TestResponse>>(),
                         Arg.Any<RequestPipelineDelegate<TestRequest, TestResponse>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior1);
                         await ci.ArgAt<RequestPipelineDelegate<TestRequest, TestResponse>>(1)(
                             ci.ArgAt<IRequestContext<TestRequest, TestResponse>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var behavior2 = Substitute.For<IRequestPipelineBehavior<TestRequest, TestResponse>>();
        behavior2.When(
                     b => b.HandleAsync(
                         Arg.Any<IRequestContext<TestRequest, TestResponse>>(),
                         Arg.Any<RequestPipelineDelegate<TestRequest, TestResponse>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior2);
                         await ci.ArgAt<RequestPipelineDelegate<TestRequest, TestResponse>>(1)(
                             ci.ArgAt<IRequestContext<TestRequest, TestResponse>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var request = new TestRequest();
        Type requestType = typeof(TestRequest);

        var descriptor = new RequestDescriptor(requestType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IRequestDescriptorFactory>();
        descriptorFactory.CreateDescriptor(requestType)
                         .Returns(descriptor);

        var pipeline = new RequestPipeline<TestRequest, TestResponse>(
            descriptorFactory,
            new[] { behavior1, behavior2 },
            Substitute.For<IRequestHandler<TestRequest, TestResponse>>(),
            Substitute.For<ILogger<RequestPipeline<TestRequest, TestResponse>>>());

        await pipeline.InvokeAsync(request);

        await behavior1.Received(1)
                       .HandleAsync(
                           Arg.Is<IRequestContext<TestRequest, TestResponse>>(
                               i => i.Request == request && i.RequestDescriptor == descriptor),
                           Arg.Any<RequestPipelineDelegate<TestRequest, TestResponse>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[0]
            .Should()
            .Be(behavior1);

        await behavior2.Received(1)
                       .HandleAsync(
                           Arg.Is<IRequestContext<TestRequest, TestResponse>>(
                               i => i.Request == request && i.RequestDescriptor == descriptor),
                           Arg.Any<RequestPipelineDelegate<TestRequest, TestResponse>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[1]
            .Should()
            .Be(behavior2);
    }

    [Fact]
    public async Task AssignsContext()
    {
        var handler = Substitute.For<IRequestHandler<TestRequest, TestResponse>>();
        var accessor = Substitute.For<IRequestContextAccessor>();

        var request = new TestRequest();
        Type requestType = typeof(TestRequest);

        var descriptor = new RequestDescriptor(requestType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<IRequestDescriptorFactory>();
        descriptorFactory.CreateDescriptor(requestType)
                         .Returns(descriptor);

        var pipeline = new RequestPipeline<TestRequest, TestResponse>(
            descriptorFactory,
            Enumerable.Empty<IRequestPipelineBehavior<TestRequest, TestResponse>>(),
            handler,
            Substitute.For<ILogger<RequestPipeline<TestRequest, TestResponse>>>(),
            accessor);

        await pipeline.InvokeAsync(request);

        accessor.Received(1)
                .CurrentContext = Arg.Is<IRequestContext<TestRequest, TestResponse>>(
            c => c.Request == request && c.RequestDescriptor == descriptor);

        accessor.Received(1)
                .CurrentContext = null;
    }
}