// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Mediator.Pipeline;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CancelableEventMetadataProviderTests
{
    [Fact]
    public void GetMetadataResolvesMetadataItemFromTypeWithAttribute()
    {
        var provider = new CancelableEventMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(CancelableTestEvent), metadata);

        metadata.Should()
                .Contain(
                    new KeyValuePair<string, object>(CancelableEventBehavior.IsCancelableMetadataKey, true));
    }

    [Fact]
    public void GetMetadataDoesNotResolveMetadataItemFromTypeWithoutAttribute()
    {
        var provider = new CancelableEventMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(TestEvent), metadata);

        metadata.Should()
                .NotContain(
                    new KeyValuePair<string, object>(CancelableEventBehavior.IsCancelableMetadataKey, true));
    }
}