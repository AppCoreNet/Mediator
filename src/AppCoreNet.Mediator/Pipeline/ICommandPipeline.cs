// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;

namespace AppCoreNet.Mediator.Pipeline;

internal interface ICommandPipeline<TResult>
{
    ICommandContext CreateCommandContext(CommandDescriptor descriptor, ICommand<TResult> command);

    Task<TResult> InvokeAsync(ICommandContext context, CancellationToken cancellationToken);
}