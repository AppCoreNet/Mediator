﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class NotificationDescriptorTests
{
    [Fact]
    public void CtorThrowsForIncompatibleType()
    {
        void Action()
        {
            _ = new NotificationDescriptor(typeof(TestRequest), new Dictionary<string, object>());
        }

        Assert.Throws<ArgumentException>(Action);
    }
}