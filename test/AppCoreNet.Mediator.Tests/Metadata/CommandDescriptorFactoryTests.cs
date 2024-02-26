// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CommandDescriptorFactoryTests
{
    [Fact]
    public void CreatesCommandDescriptor()
    {
        var factory = new CommandDescriptorFactory(Enumerable.Empty<ICommandMetadataProvider>());
        CommandDescriptor descriptor = factory.CreateDescriptor(typeof(TestCommand));

        descriptor.CommandType.Should()
                  .Be<TestCommand>();
    }

    [Fact]
    public void PopulatesCommandDescriptorWithMetadata()
    {
        var provider1 = Substitute.For<ICommandMetadataProvider>();
        provider1.When(p => p.GetMetadata(Arg.Any<Type>(), Arg.Any<IDictionary<string, object>>()))
                 .Do(
                     ci =>
                     {
                         var metadata = ci.ArgAt<IDictionary<string, object>>(1);
                         metadata.Add("1", 1);
                     });

        var provider2 = Substitute.For<ICommandMetadataProvider>();
        provider2.When(p => p.GetMetadata(Arg.Any<Type>(), Arg.Any<IDictionary<string, object>>()))
                 .Do(
                     ci =>
                     {
                         var metadata = ci.ArgAt<IDictionary<string, object>>(1);
                         metadata.Add("2", 2);
                     });

        var factory = new CommandDescriptorFactory(
            new[]
            {
                provider1,
                provider2,
            });

        CommandDescriptor descriptor = factory.CreateDescriptor(typeof(TestCommand));
        descriptor.Metadata.Should()
                  .BeEquivalentTo(
                      new[]
                      {
                          new KeyValuePair<string, object>("1", 1),
                          new KeyValuePair<string, object>("2", 2),
                      });
    }
}