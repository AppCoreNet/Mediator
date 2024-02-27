// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class RequestDescriptorFactoryTests
{
    [Fact]
    public void CreatesDescriptor()
    {
        var factory = new RequestDescriptorFactory(Enumerable.Empty<IRequestMetadataProvider>());
        RequestDescriptor descriptor = factory.CreateDescriptor(typeof(TestRequest));

        descriptor.RequestType.Should()
                  .Be<TestRequest>();
    }

    [Fact]
    public void PopulatesDescriptorWithMetadata()
    {
        var provider1 = Substitute.For<IRequestMetadataProvider>();
        provider1.When(p => p.GetMetadata(Arg.Any<Type>(), Arg.Any<IDictionary<string, object>>()))
                 .Do(
                     ci =>
                     {
                         var metadata = ci.ArgAt<IDictionary<string, object>>(1);
                         metadata.Add("1", 1);
                     });

        var provider2 = Substitute.For<IRequestMetadataProvider>();
        provider2.When(p => p.GetMetadata(Arg.Any<Type>(), Arg.Any<IDictionary<string, object>>()))
                 .Do(
                     ci =>
                     {
                         var metadata = ci.ArgAt<IDictionary<string, object>>(1);
                         metadata.Add("2", 2);
                     });

        var factory = new RequestDescriptorFactory(
            new[]
            {
                provider1,
                provider2,
            });

        RequestDescriptor descriptor = factory.CreateDescriptor(typeof(TestRequest));
        descriptor.Metadata.Should()
                  .BeEquivalentTo(
                      new[]
                      {
                          new KeyValuePair<string, object>("1", 1),
                          new KeyValuePair<string, object>("2", 2),
                      });
    }
}