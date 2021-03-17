using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieDatabase.CommandStack.EventHandlers;
using SimpleInjector;

namespace MovieDatabase.Services.EventProcessing
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Container _container;

        public EventDispatcher(Container container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public async Task DispatchEvent <TEvent>(TEvent eventData) where TEvent : EventBase, new()
        {
            var ct = CancellationToken.None;
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventData.GetType());
            var eventHandlers = _container.GetAllInstances(handlerType);

            foreach (var eventHandler in eventHandlers)
            {
                var casted = (IEventHandler<TEvent>) eventHandler;
                await casted.HandleEvent(eventData, ct);
            }
        }
    }
}
