// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Default implementation of the <see cref="IRequestContext{TRequest,TResponse}"/> interface.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class RequestContext<TRequest, TResponse> : IRequestContext<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <inheritdoc />
    public RequestDescriptor RequestDescriptor { get; }

    /// <inheritdoc />
    public TRequest Request { get; }

    /// <inheritdoc />
    public TResponse? Response { get; private set; }

    /// <inheritdoc />
    public Exception? Error { get; private set; }

    /// <inheritdoc/>
    object? IRequestContext.Response => Response;

    /// <inheritdoc />
    public bool IsCompleted { get; private set; }

    /// <inheritdoc />
    public bool IsFailed => Error != null;

    /// <inheritdoc />
    IRequest<object> IRequestContext.Request => (IRequest<object>)Request;

    /// <inheritdoc />
    public IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

    /// <inheritdoc />
    public IDictionary<Type, object> Features { get; } = new Dictionary<Type, object>();

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestContext{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="RequestDescriptor"/>.</param>
    /// <param name="command">The command that is being processed.</param>
    public RequestContext(RequestDescriptor descriptor, TRequest command)
    {
        Ensure.Arg.NotNull(descriptor);
        Ensure.Arg.OfType<TRequest>(descriptor.RequestType, nameof(descriptor));
        Ensure.Arg.NotNull(command);

        RequestDescriptor = descriptor;
        Request = command;
    }

    void IRequestContext.Complete(object response)
    {
        Complete((TResponse)response);
    }

    /// <inheritdoc />
    public void Fail(Exception error)
    {
        Ensure.Arg.NotNull(error);
        IsCompleted = true;
        Error = error;
    }

    /// <inheritdoc />
    public void Complete(TResponse response)
    {
        Ensure.Arg.NotNull(response);
        IsCompleted = true;
        Response = response;
    }
}