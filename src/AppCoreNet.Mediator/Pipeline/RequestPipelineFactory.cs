// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Mediator.Pipeline;

/// <summary>
/// Provides a factory for <see cref="IRequestPipeline{TResponse}"/>.
/// </summary>
public sealed class RequestPipelineFactory : IRequestPipelineFactory
{
    private readonly IActivator _activator;
    private static readonly Type _pipelineType = typeof(RequestPipeline<,>);
    private static readonly ConcurrentDictionary<Type, Type> _pipelineTypes = new ();

    private static Type GetRequestPipelineType(Type requestType)
    {
        return _pipelineTypes.GetOrAdd(requestType, t =>
        {
            Type requestInterfaceType = t.GetClosedTypeOf(typeof(IRequest<>));
            return _pipelineType.MakeGenericType(t, requestInterfaceType.GenericTypeArguments[0]);
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPipelineFactory"/> class.
    /// </summary>
    /// <param name="activator">The <see cref="IActivator"/>.</param>
    public RequestPipelineFactory(IActivator activator)
    {
        Ensure.Arg.NotNull(activator);
        _activator = activator;
    }

    /// <inheritdoc/>
    public IRequestPipeline<TResponse> CreatePipeline<TResponse>(IRequest<TResponse> request)
    {
        Ensure.Arg.NotNull(request);
        Type pipelineType = GetRequestPipelineType(request.GetType());
        return (IRequestPipeline<TResponse>)_activator.CreateInstance(pipelineType) !;
    }
}