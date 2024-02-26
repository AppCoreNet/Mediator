// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator;

public class CommandContextTests
{
    [Fact]
    public void CtorThrowsIfCommandDoesNotMatchDescriptorType()
    {
        Type commandType = typeof(TestCommand);
        var descriptor = new CommandDescriptor(commandType, new Dictionary<string, object>());

        Action action = () =>
        {
            // ReSharper disable once ObjectCreationAsStatement
            new CommandContext<CancelableTestCommand, TestResult>(descriptor, new CancelableTestCommand());
        };

        action.Should()
              .Throw<ArgumentException>();
    }

    [Fact]
    public void GetCommandReturnsCommand()
    {
        Type commandType = typeof(TestCommand);
        var descriptor = new CommandDescriptor(commandType, new Dictionary<string, object>());
        var command = new TestCommand();
        var context = new CommandContext<TestCommand, TestResult>(descriptor, command);

        context.Command.Should()
               .BeSameAs(command);

        ((ICommandContext)context).Command.Should()
                                  .BeSameAs(command);
    }
}