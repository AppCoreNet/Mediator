// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Represents a delegate in the request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The response produced by the request.</typeparam>
/// <param name="request">The request.</param>
/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous request operation.</returns>
public delegate Task RequestPipelineDelegate<in TRequest, TResponse>(
    IRequestContext<TRequest, TResponse> request,
    CancellationToken cancellationToken)
    where TRequest : IRequest<TResponse>;