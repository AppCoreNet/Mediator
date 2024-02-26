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

public class CommandPipelineTests
{
    [Fact]
    public async Task InvokesHandlers()
    {
        var handler1 = Substitute.For<ICommandHandler<TestCommand, TestResult>>();

        var command = new TestCommand();
        Type commandType = typeof(TestCommand);

        var descriptor = new CommandDescriptor(commandType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<ICommandDescriptorFactory>();
        descriptorFactory.CreateDescriptor(commandType)
                         .Returns(descriptor);

        var pipeline = new CommandPipeline<TestCommand, TestResult>(
            descriptorFactory,
            Enumerable.Empty<ICommandPipelineBehavior<TestCommand, TestResult>>(),
            handler1,
            Substitute.For<ILogger<CommandPipeline<TestCommand, TestResult>>>());

        await pipeline.InvokeAsync(command);

        await handler1.Received(1)
                      .HandleAsync(
                          Arg.Is(command),
                          Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokesBehaviors()
    {
        var invokedBehaviors = new List<ICommandPipelineBehavior<TestCommand, TestResult>>();

        var behavior1 = Substitute.For<ICommandPipelineBehavior<TestCommand, TestResult>>();
        behavior1.When(
                     b => b.HandleAsync(
                         Arg.Any<ICommandContext<TestCommand, TestResult>>(),
                         Arg.Any<CommandPipelineDelegate<TestCommand, TestResult>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior1);
                         await ci.ArgAt<CommandPipelineDelegate<TestCommand, TestResult>>(1)(
                             ci.ArgAt<ICommandContext<TestCommand, TestResult>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var behavior2 = Substitute.For<ICommandPipelineBehavior<TestCommand, TestResult>>();
        behavior2.When(
                     b => b.HandleAsync(
                         Arg.Any<ICommandContext<TestCommand, TestResult>>(),
                         Arg.Any<CommandPipelineDelegate<TestCommand, TestResult>>(),
                         Arg.Any<CancellationToken>()))
                 .Do(
                     async ci =>
                     {
                         invokedBehaviors.Add(behavior2);
                         await ci.ArgAt<CommandPipelineDelegate<TestCommand, TestResult>>(1)(
                             ci.ArgAt<ICommandContext<TestCommand, TestResult>>(0),
                             ci.ArgAt<CancellationToken>(2));
                     });

        var command = new TestCommand();
        Type commandType = typeof(TestCommand);

        var descriptor = new CommandDescriptor(commandType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<ICommandDescriptorFactory>();
        descriptorFactory.CreateDescriptor(commandType)
                         .Returns(descriptor);

        var pipeline = new CommandPipeline<TestCommand, TestResult>(
            descriptorFactory,
            new[] { behavior1, behavior2 },
            Substitute.For<ICommandHandler<TestCommand, TestResult>>(),
            Substitute.For<ILogger<CommandPipeline<TestCommand, TestResult>>>());

        await pipeline.InvokeAsync(command);

        await behavior1.Received(1)
                       .HandleAsync(
                           Arg.Is<ICommandContext<TestCommand, TestResult>>(
                               i => i.Command == command && i.CommandDescriptor == descriptor),
                           Arg.Any<CommandPipelineDelegate<TestCommand, TestResult>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[0]
            .Should()
            .Be(behavior1);

        await behavior2.Received(1)
                       .HandleAsync(
                           Arg.Is<ICommandContext<TestCommand, TestResult>>(
                               i => i.Command == command && i.CommandDescriptor == descriptor),
                           Arg.Any<CommandPipelineDelegate<TestCommand, TestResult>>(),
                           Arg.Any<CancellationToken>());

        invokedBehaviors[1]
            .Should()
            .Be(behavior2);
    }

    [Fact]
    public async Task AssignsCommandContext()
    {
        var handler = Substitute.For<ICommandHandler<TestCommand, TestResult>>();
        var accessor = Substitute.For<ICommandContextAccessor>();

        var command = new TestCommand();
        Type commandType = typeof(TestCommand);

        var descriptor = new CommandDescriptor(commandType, new Dictionary<string, object>());

        var descriptorFactory = Substitute.For<ICommandDescriptorFactory>();
        descriptorFactory.CreateDescriptor(commandType)
                         .Returns(descriptor);

        var pipeline = new CommandPipeline<TestCommand, TestResult>(
            descriptorFactory,
            Enumerable.Empty<ICommandPipelineBehavior<TestCommand, TestResult>>(),
            handler,
            Substitute.For<ILogger<CommandPipeline<TestCommand, TestResult>>>(),
            accessor);

        await pipeline.InvokeAsync(command);

        accessor.Received(1)
                .CommandContext = Arg.Is<ICommandContext<TestCommand, TestResult>>(
            c => c.Command == command && c.CommandDescriptor == descriptor);

        accessor.Received(1)
                .CommandContext = null;
    }
}