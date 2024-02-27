// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator;

public class NotificationContextTests
{
    [Fact]
    public void ConstructorThrowsForIncompatibleArguments()
    {
        Action action = () =>
        {
            _ = new NotificationContext<TestNotification>(
                new NotificationDescriptor(typeof(CancelableTestNotification), new Dictionary<string, object>()),
                new TestNotification());
        };

        action.Should()
              .Throw<ArgumentException>();
    }
}