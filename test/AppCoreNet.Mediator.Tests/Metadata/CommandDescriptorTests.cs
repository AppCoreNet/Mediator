using System;
using System.Collections.Generic;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CommandDescriptorTests
{
    [Fact]
    public void CtorThrowsForIncompatibleType()
    {
        void Action()
        {
            _ = new CommandDescriptor(typeof(TestEvent), new Dictionary<string, object>());
        }

        Assert.Throws<ArgumentException>(Action);
    }
}