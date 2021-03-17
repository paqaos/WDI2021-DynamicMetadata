using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.Services.EventProcessing;

namespace MovieDatabase.CommandStack.EventHandlers
{
    public interface IEventHandler <in TEvent> where TEvent : new()
    {
        Task HandleEvent(TEvent eventData, CancellationToken ct);
    }
}
