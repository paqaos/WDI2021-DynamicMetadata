using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services.EventProcessing
{
    public interface IEventDispatcher
    {
        Task DispatchEvent <TEvent>(TEvent eventData) where TEvent : EventBase, new();
    }
}
