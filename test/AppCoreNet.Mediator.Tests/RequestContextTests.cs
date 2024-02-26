// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator;

public class RequestContextTests
{
    [Fact]
    public void CtorThrowsIfCommandDoesNotMatchDescriptorType()
    {
        Type commandType = typeof(TestRequest);
        var descriptor = new RequestDescriptor(commandType, new Dictionary<string, object>());

        Action action = () =>
        {
            // ReSharper disable once ObjectCreationAsStatement
            new RequestContext<CancelableTestRequest, TestResponse>(descriptor, new CancelableTestRequest());
        };

        action.Should()
              .Throw<ArgumentException>();
    }

    [Fact]
    public void GetRequestReturnsRequest()
    {
        Type commandType = typeof(TestRequest);
        var descriptor = new RequestDescriptor(commandType, new Dictionary<string, object>());
        var command = new TestRequest();
        var context = new RequestContext<TestRequest, TestResponse>(descriptor, command);

        context.Request.Should()
               .BeSameAs(command);

        ((IRequestContext)context).Request.Should()
                                  .BeSameAs(command);
    }
}