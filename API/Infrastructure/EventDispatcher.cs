using API.Handlers;
using SignalR;

namespace API.Infrastructure
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _provider;

        public EventDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task DispatchAsync<TEvent>(TEvent domainEvent)
        {
            var handlers = _provider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
                await handler.HandleAsync(domainEvent);
        }
    }
}
