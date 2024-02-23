// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using AppCore.CommandModel.Metadata;

namespace AppCore.CommandModel.Pipeline;

internal class CommandPipeline<TCommand, TResult> : ICommandPipeline<TResult>
    where TCommand : ICommand<TResult>
{
    private readonly IEnumerable<ICommandPipelineBehavior<TCommand, TResult>> _behaviors;
    private readonly ICommandHandler<TCommand, TResult> _handler;

    public CommandPipeline(IEnumerable<ICommandPipelineBehavior<TCommand, TResult>> behaviors, ICommandHandler<TCommand, TResult> handler)
    {
        _behaviors = behaviors;
        _handler = handler;
    }

    public ICommandContext CreateCommandContext(CommandDescriptor descriptor, ICommand<TResult> command)
    {
        return new CommandContext<TCommand, TResult>(descriptor, (TCommand) command);
    }

    public async Task<TResult> InvokeAsync(ICommandContext context, CancellationToken cancellationToken)
    {
        ExceptionDispatchInfo? exceptionDispatchInfo = null;

        await _behaviors
              .Reverse()
              .Aggregate(
                  (CommandPipelineDelegate<TCommand, TResult>) (async (c, ct) =>
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

                      await behavior.ProcessAsync(c, next, ct)
                                    .ConfigureAwait(false);
                  })(
                  (ICommandContext<TCommand, TResult>) context,
                  cancellationToken);

        if (context.IsFailed)
        {
            exceptionDispatchInfo?.Throw();
            throw context.Error!;
        }

        return ((ICommandContext<TCommand, TResult>) context).Result!;
    }
}