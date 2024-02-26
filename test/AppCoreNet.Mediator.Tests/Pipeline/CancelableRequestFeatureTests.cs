// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class CancelableRequestFeatureTests
{
    [Fact]
    public void CancelSignalsCancellationTokenSource()
    {
        using var cts = new CancellationTokenSource();
        var feature = new CancelableRequestFeature(cts);
        feature.Cancel();

        cts.IsCancellationRequested.Should()
           .BeTrue();
    }
}