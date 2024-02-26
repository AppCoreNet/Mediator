﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Mediator;

/// <summary>
/// Represents a request.
/// </summary>
/// <typeparam name="TResponse">The response.</typeparam>
public interface IRequest<out TResponse>
{
}

/// <summary>
/// Represents a request without a result.
/// </summary>
public interface IRequest : IRequest<VoidResult>
{
}