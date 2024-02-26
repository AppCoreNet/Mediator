using System;
using System.Collections.Generic;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class RequestDescriptorTests
{
    [Fact]
    public void CtorThrowsForIncompatibleType()
    {
        void Action()
        {
            _ = new RequestDescriptor(typeof(TestEvent), new Dictionary<string, object>());
        }

        Assert.Throws<ArgumentException>(Action);
    }
}