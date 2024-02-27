// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class CancelableRequestContextExtensionsTests
{
    private readonly IRequestContext<TestRequest, TestResponse> _context;
    private readonly Dictionary<Type, object> _features;

    public CancelableRequestContextExtensionsTests()
    {
        _context = Substitute.For<IRequestContext<TestRequest, TestResponse>>();
        _features = new Dictionary<Type, object>();
        _context.Features.Returns(_features);
    }

    [Fact]
    public void IsCancelableReturnsTrueWhenFeatureIsPresent()
    {
        _features.Add(
            typeof(ICancelableRequestFeature),
            Substitute.For<ICancelableRequestFeature>());

        _context.IsCancelable()
                .Should()
                .BeTrue();
    }

    [Fact]
    public void IsCancelableReturnsFalseWhenFeatureIsNotPresent()
    {
        _context.IsCancelable()
                .Should()
                .BeFalse();
    }

    [Fact]
    public void CancelInvokesCancelOnFeature()
    {
        var feature = Substitute.For<ICancelableRequestFeature>();
        _features.Add(typeof(ICancelableRequestFeature), feature);
        _context.Cancel();

        feature.Received(1)
               .Cancel();
    }
}