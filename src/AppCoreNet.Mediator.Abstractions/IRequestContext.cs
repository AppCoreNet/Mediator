// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents the context when processing a request.
/// </summary>
public interface IRequestContext
{
    /// <summary>
    /// Gets the <see cref="RequestDescriptor"/> of the request.
    /// </summary>
    RequestDescriptor RequestDescriptor { get; }

    /// <summary>
    /// Gets the request.
    /// </summary>
    IRequest<object> Request { get; }

    /// <summary>
    /// Gets the command response.
    /// </summary>
    object? Response { get; }

    /// <summary>
    /// Gets the error that occured.
    /// </summary>
    Exception? Error { get; }

    /// <summary>
    /// Gets a value indicating whether the request has been completed (successfully or not).
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Gets a value indicating whether the request has failed.
    /// </summary>
    bool IsFailed { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context items.
    /// </summary>
    IDictionary<object, object> Items { get; }

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> of context features.
    /// </summary>
    IDictionary<Type, object> Features { get; }

    /// <summary>
    /// Completes the request with the specified response.
    /// </summary>
    /// <param name="response">The command response.</param>
    void Complete(object response);

    /// <summary>
    /// Completes the request with the specified <see cref="Exception"/>.
    /// </summary>
    /// <param name="error">The <see cref="Exception"/> which occured during processing.</param>
    void Fail(Exception error);
}

/// <summary>
/// Represents typed context when processing a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestContext<out TRequest, TResponse> : IRequestContext
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Gets the request.
    /// </summary>
    new TRequest Request { get; }

    /// <summary>
    /// Gets the request response.
    /// </summary>
    new TResponse? Response { get; }

    /// <summary>
    /// Completes the request with the specified response.
    /// </summary>
    /// <param name="response">The request response.</param>
    void Complete(TResponse response);
}