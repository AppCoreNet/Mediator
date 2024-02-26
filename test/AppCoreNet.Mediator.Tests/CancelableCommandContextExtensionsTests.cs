// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Pipeline;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator;

public class CancelableCommandContextExtensionsTests
{
    private readonly ICommandContext<TestCommand, TestResult> _context;
    private readonly Dictionary<Type, object> _features;

    public CancelableCommandContextExtensionsTests()
    {
        _context = Substitute.For<ICommandContext<TestCommand, TestResult>>();
        _features = new Dictionary<Type, object>();
        _context.Features.Returns(_features);
    }

    [Fact]
    public void IsCancelableReturnsTrueWhenFeatureIsPresent()
    {
        _features.Add(
            typeof(ICancelableCommandFeature),
            Substitute.For<ICancelableCommandFeature>());

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
        var feature = Substitute.For<ICancelableCommandFeature>();
        _features.Add(typeof(ICancelableCommandFeature), feature);
        _context.Cancel();

        feature.Received(1)
               .Cancel();
    }
}