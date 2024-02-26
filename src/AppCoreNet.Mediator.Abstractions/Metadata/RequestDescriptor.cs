// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Mediator.Metadata;

/// <summary>
/// Describes a request type.
/// </summary>
public class RequestDescriptor
{
    /// <summary>
    /// Gets the type of the request.
    /// </summary>
    public Type RequestType { get; }

    /// <summary>
    /// Gets the metadata of the request type.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestDescriptor"/> class.
    /// </summary>
    /// <param name="requestType">The type of the request.</param>
    /// <param name="metadata">The request type metadata.</param>
    public RequestDescriptor(Type requestType, IReadOnlyDictionary<string, object> metadata)
    {
        Ensure.Arg.NotNull(requestType);
        Ensure.Arg.OfType(requestType, typeof(IRequest<>));
        Ensure.Arg.NotNull(metadata);

        RequestType = requestType;
        Metadata = metadata;
    }
}