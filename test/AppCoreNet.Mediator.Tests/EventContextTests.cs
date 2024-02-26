// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator;

public class EventContextTests
{
    [Fact]
    public void ConstructorThrowsForIncompatibleArguments()
    {
        Action action = () =>
        {
            _ = new EventContext<TestEvent>(
                new EventDescriptor(typeof(CancelableTestEvent), new Dictionary<string, object>()),
                new TestEvent());
        };

        action.Should()
              .Throw<ArgumentException>();
    }
}