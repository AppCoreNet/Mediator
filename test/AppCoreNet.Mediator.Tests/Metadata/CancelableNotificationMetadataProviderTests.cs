// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Mediator.Pipeline;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class CancelableNotificationMetadataProviderTests
{
    [Fact]
    public void GetMetadataResolvesMetadataItemFromTypeWithAttribute()
    {
        var provider = new CancelableNotificationMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(CancelableTestNotification), metadata);

        metadata.Should()
                .Contain(
                    new KeyValuePair<string, object>(CancelableNotificationBehavior.IsCancelableMetadataKey, true));
    }

    [Fact]
    public void GetMetadataDoesNotResolveMetadataItemFromTypeWithoutAttribute()
    {
        var provider = new CancelableNotificationMetadataProvider();

        var metadata = new Dictionary<string, object>();
        provider.GetMetadata(typeof(TestNotification), metadata);

        metadata.Should()
                .NotContain(
                    new KeyValuePair<string, object>(CancelableNotificationBehavior.IsCancelableMetadataKey, true));
    }
}