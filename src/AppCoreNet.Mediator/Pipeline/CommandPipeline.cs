// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Mediator.Metadata;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Mediator.Pipeline;

public sealed class CommandPipeline<TCommand, TResult> : ICommandPipeline<TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandDescriptorFactory _descriptorFactory;
    private readonly IEnumerable<ICommandPipelineBehavior<TCommand, TResult>> _behaviors;
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly ILogger<CommandPipeline<TCommand, TResult>> _logger;
    private readonly ICommandContextAccessor? _contextAccessor;

    public CommandPipeline(
        ICommandDescriptorFactory descriptorFactory,
        IEnumerable<ICommandPipelineBehavior<TCommand, TResult>> behaviors,
        ICommandHandler<TCommand, TResult> handler,
        ILogger<CommandPipeline<TCommand, TResult>> logger,
        ICommandContextAccessor? contextAccessor = null)
    {
        _descriptorFactory = descriptorFactory;
        _behaviors = behaviors;
        _handler = handler;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    public async Task<TResult> InvokeAsync(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        CommandDescriptor descriptor = _descriptorFactory.CreateDescriptor(typeof(TCommand));
        var context = new CommandContext<TCommand, TResult>(descriptor, (TCommand)command);

        if (_contextAccessor != null)
            _contextAccessor.CommandContext = context;

        try
        {
            return await InvokeAsync(context, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            if (_contextAccessor != null)
                _contextAccessor.CommandContext = null;
        }
    }

    private async Task<TResult> InvokeAsync(ICommandContext<TCommand, TResult> context, CancellationToken cancellationToken)
    {
        ExceptionDispatchInfo? exceptionDispatchInfo = null;

        await _behaviors
              .Reverse()
              .Aggregate(
                  (CommandPipelineDelegate<TCommand, TResult>)(async (c, ct) =>
                  {
                      if (!c.IsCompleted)
                      {
                          ct.ThrowIfCancellationRequested();

                          try
                          {
                              TResult result = await _handler.HandleAsync(c.Command, ct)
                                                             .ConfigureAwait(false);

                              c.Complete(result);
                          }
                          catch (Exception error)
                          {
                              exceptionDispatchInfo = ExceptionDispatchInfo.Capture(error);
                              c.Fail(error);
                          }
                      }
                  }),
                  (next, behavior) => async (c, ct) =>
                  {
                      ct.ThrowIfCancellationRequested();

                      await behavior.HandleAsync(c, next, ct)
                                    .ConfigureAwait(false);
                  })(
                  context,
                  cancellationToken);

        if (context.IsFailed)
        {
            exceptionDispatchInfo?.Throw();
            throw context.Error!;
        }

        return context.Result!;
    }
}