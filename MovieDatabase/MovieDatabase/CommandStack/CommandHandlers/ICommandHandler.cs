using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.Commands;

namespace MovieDatabase.CommandStack.CommandHandlers
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
    {
        public Task<TResult> HandleResult(TCommand command, CancellationToken token);
    }
}
