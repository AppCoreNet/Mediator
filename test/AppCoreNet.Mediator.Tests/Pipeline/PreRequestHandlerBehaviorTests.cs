// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class PreRequestHandlerBehaviorTests
{
    [Fact]
    public async Task InvokesHandlersBeforeNext()
    {
        var invokeOrder = new List<object>();

        var handlers = new[]
        {
            Substitute.For<IPreRequestHandler<TestRequest, TestResponse>>(),
            Substitute.For<IPreRequestHandler<TestRequest, TestResponse>>(),
        };

        handlers[0]
            .When(h => h.OnHandlingAsync(Arg.Any<IRequestContext<TestRequest, TestResponse>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[0]));

        handlers[1]
            .When(h => h.OnHandlingAsync(Arg.Any<IRequestContext<TestRequest, TestResponse>>(), Arg.Any<CancellationToken>()))
            .Do(_ => invokeOrder.Add(handlers[1]));

        var next = Substitute.For<RequestPipelineDelegate<TestRequest, TestResponse>>();
        next.When(n => n.Invoke(Arg.Any<IRequestContext<TestRequest, TestResponse>>(), Arg.Any<CancellationToken>()))
            .Do(_ => { invokeOrder.Add(next); });

        var command = new TestRequest();
        var context = new RequestContext<TestRequest, TestResponse>(
            new RequestDescriptor(typeof(TestRequest), new Dictionary<string, object>()),
            command);

        var behavior = new PreRequestHandlerBehavior<TestRequest, TestResponse>(handlers);
        await behavior.HandleAsync(context, next);

        await handlers[0]
              .Received(1)
              .OnHandlingAsync(
                  Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                  Arg.Any<CancellationToken>());

        invokeOrder[0]
            .Should()
            .Be(handlers[0]);

        await handlers[1]
              .Received(1)
              .OnHandlingAsync(
                  Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                  Arg.Any<CancellationToken>());

        invokeOrder[1]
            .Should()
            .Be(handlers[1]);

        await next.Received(1)
                  .Invoke(
                      Arg.Is<IRequestContext<TestRequest, TestResponse>>(context),
                      Arg.Any<CancellationToken>());

        invokeOrder[2]
            .Should()
            .Be(next);
    }
}