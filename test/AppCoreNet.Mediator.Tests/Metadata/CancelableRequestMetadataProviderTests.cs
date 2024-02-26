// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Mediator.Pipeline;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CancelableRequestMetadataProviderTests
{
    [Fact]
    public void GetMetadataResolvesMetadataItemFromTypeWithAttribute()
    {
        var provider = new CancelableRequestMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(CancelableTestRequest), metadata);

        metadata.Should()
                .Contain(
                    new KeyValuePair<string, object>(CancelableRequestBehavior.IsCancelableMetadataKey, true));
    }

    [Fact]
    public void GetMetadataDoesNotResolveMetadataItemFromTypeWithoutAttribute()
    {
        var provider = new CancelableRequestMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(TestRequest), metadata);

        metadata.Should()
                .NotContain(
                    new KeyValuePair<string, object>(CancelableRequestBehavior.IsCancelableMetadataKey, true));
    }
}