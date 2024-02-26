// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class RequestPipelineFactory : IRequestPipelineFactory
{
    private readonly IActivator _activator;
    private static readonly Type _requestPipelineType = typeof(RequestPipeline<,>);
    private static readonly ConcurrentDictionary<Type, Type> _requestPipelineTypes = new ();

    private static Type GetRequestPipelineType(Type requestType)
    {
        return _requestPipelineTypes.GetOrAdd(requestType, t =>
        {
            Type commandInterfaceType = t.GetClosedTypeOf(typeof(IRequest<>));
            return _requestPipelineType.MakeGenericType(t, commandInterfaceType.GenericTypeArguments[0]);
        });
    }

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