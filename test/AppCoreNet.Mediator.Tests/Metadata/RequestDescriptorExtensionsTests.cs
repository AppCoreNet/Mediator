// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Mediator.Metadata;

public class RequestDescriptorExtensionsTests
{
    [Fact]
    public void TryGetMetadataReturnsValue()
    {
        string testKey = "test";
        int testValue = 1;

        var metadata = new Dictionary<string, object> { { testKey, testValue } };
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        descriptor.TryGetMetadata(testKey, out int value)
                  .Should()
                  .BeTrue();

        value.Should()
             .Be(testValue);
    }

    [Fact]
    public void TryGetMetadataReturnsFalseIfNotFound()
    {
        string testKey = "test";
        var metadata = new Dictionary<string, object>();
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        descriptor.TryGetMetadata(testKey, out int _)
                  .Should()
                  .BeFalse();
    }

    [Fact]
    public void TryGetMetadataThrowsIfOfWrongType()
    {
        string testKey = "test";
        int testValue = 1;

        var metadata = new Dictionary<string, object> { { testKey, testValue } };
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        Action action = () =>
        {
            descriptor.TryGetMetadata(testKey, out string? _);
        };

        action.Should()
              .Throw<InvalidCastException>();
    }

    [Fact]
    public void GetMetadataReturnsDefaultValueIfNotFound()
    {
        int defaultValue = 123;
        string testKey = "test";
        var metadata = new Dictionary<string, object>();
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        descriptor.GetMetadata(testKey, defaultValue)
                  .Should()
                  .Be(defaultValue);
    }

    [Fact]
    public void GetMetadataReturnsValue()
    {
        string testKey = "test";
        int testValue = 1;

        var metadata = new Dictionary<string, object> { { testKey, testValue } };
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        descriptor.GetMetadata<int>(testKey)
                  .Should()
                  .Be(testValue);
    }

    [Fact]
    public void GetMetadataThrowsIfNotFound()
    {
        var metadata = new Dictionary<string, object>();
        var descriptor = new RequestDescriptor(typeof(TestRequest), metadata);

        Action action = () =>
        {
            descriptor.GetMetadata<int>("key");
        };

        action.Should()
              .Throw<KeyNotFoundException>();
    }
}