// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Mediator.Pipeline;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CancelableCommandMetadataProviderTests
{
    [Fact]
    public void GetMetadataResolvesMetadataItemFromTypeWithAttribute()
    {
        var provider = new CancelableCommandMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(CancelableTestCommand), metadata);

        metadata.Should()
                .Contain(
                    new KeyValuePair<string, object>(CancelableCommandBehavior.IsCancelableMetadataKey, true));
    }

    [Fact]
    public void GetMetadataDoesNotResolveMetadataItemFromTypeWithoutAttribute()
    {
        var provider = new CancelableCommandMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(TestCommand), metadata);

        metadata.Should()
                .NotContain(
                    new KeyValuePair<string, object>(CancelableCommandBehavior.IsCancelableMetadataKey, true));
    }
}