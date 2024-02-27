// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class RequestContextTests
{
    [Fact]
    public void CtorThrowsIfRequestDoesNotMatchDescriptorType()
    {
        Type requestType = typeof(TestRequest);
        var descriptor = new RequestDescriptor(requestType, new Dictionary<string, object>());

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
        Type requestType = typeof(TestRequest);
        var descriptor = new RequestDescriptor(requestType, new Dictionary<string, object>());
        var request = new TestRequest();
        var context = new RequestContext<TestRequest, TestResponse>(descriptor, request);

        context.Request.Should()
               .BeSameAs(request);

        ((IRequestContext)context).Request.Should()
                                  .BeSameAs(request);
    }
}