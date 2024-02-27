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

public class PostRequestHandlerBehaviorTests
{
    [Fact]
    public async Task InvokesHandlersAfterNext()
    {
        var invokeOrder = new List<object>();

        var handlers = new[]
        {
            Substitute.For<IPostRequestHandler<TestRequest, TestResponse>>(),
            Substitute.For<IPostRequestHandler<TestRequest, TestResponse>>(),
        };

        handlers[0]
            .When(
                h => h.OnHandledAsync(
                    Arg.Any<IRequestContext<TestRequest, TestResponse>>(),
                    Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[0]));

        handlers[1]
            .When(
                h => h.OnHandledAsync(
                    Arg.Any<IRequestContext<TestRequest, TestResponse>>(),
                    Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[1]));

        var next = Substitute.For<RequestPipelineDelegate<TestRequest, TestResponse>>();
        next.When(n => n.Invoke(Arg.Any<IRequestContext<TestRequest, TestResponse>>(), Arg.Any<CancellationToken>()))
            .Do(_ => { invokeOrder.Add(next); });

        var request = new TestRequest();
        var context = new RequestContext<TestRequest, TestResponse>(
            new RequestDescriptor(typeof(TestRequest), new Dictionary<string, object>()),
            request);

        var behavior = new PostRequestHandlerBehavior<TestRequest, TestResponse>(
            handlers,
            Substitute.For<ILogger<PostRequestHandlerBehavior<TestRequest, TestResponse>>>());

        await behavior.HandleAsync(context, next);

        await next.Received(1)
                  .Invoke(
                      Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                      Arg.Any<CancellationToken>());

        invokeOrder[0]
            .Should()
            .Be(next);

        await handlers[0]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                  Arg.Any<CancellationToken>());

        invokeOrder[1]
            .Should()
            .Be(handlers[0]);

        await handlers[1]
              .Received(1)
              .OnHandledAsync(
                  Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                  Arg.Any<CancellationToken>());

        invokeOrder[2]
            .Should()
            .Be(handlers[1]);
    }
}