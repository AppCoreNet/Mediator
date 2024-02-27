// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Mediator.Pipeline;

public class CancelableNotificationPipelineTests
{
    [Fact]
    public async Task CancelThrowsOperationCanceledException()
    {
        var handler = Substitute.For<INotificationHandler<CancelableTestNotification>>();

        var behavior = Substitute.For<INotificationPipelineBehavior<CancelableTestNotification>>();
        behavior.When(
                    h => h.HandleAsync(
                        Arg.Any<INotificationContext<CancelableTestNotification>>(),
                        Arg.Any<NotificationPipelineDelegate<CancelableTestNotification>>(),
                        Arg.Any<CancellationToken>()))
                .Do(
                    ci => ci.ArgAt<INotificationContext<CancelableTestNotification>>(0)
                            .Cancel());

        var metadata = new Dictionary<string, object>();
        new CancelableNotificationMetadataProvider().GetMetadata(typeof(CancelableTestNotification), metadata);

        var notification = new CancelableTestNotification();
        Type notificationType = typeof(CancelableTestNotification);

        var descriptor = new NotificationDescriptor(notificationType, metadata);

        var descriptorFactory = Substitute.For<INotificationDescriptorFactory>();
        descriptorFactory.CreateDescriptor(notificationType)
                         .Returns(descriptor);

        var pipeline = new NotificationPipeline<CancelableTestNotification>(
            descriptorFactory,
            new[] { new CancelableNotificationBehavior<CancelableTestNotification>(), behavior },
            new[] { handler },
            Substitute.For<ILogger<NotificationPipeline<CancelableTestNotification>>>());

        Func<Task> invoke = async () => { await pipeline.InvokeAsync(notification); };

        await invoke.Should()
                    .ThrowAsync<OperationCanceledException>();
    }
}