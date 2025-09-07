using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using SignalR;

namespace API.Handlers
{
    public class ChatMessageCreatedHandler : IEventHandler<ChatMessageCreatedEvent>
    {
        private readonly IHubContext<ChatHub> _hub;

        public ChatMessageCreatedHandler(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public Task HandleAsync(ChatMessageCreatedEvent domainEvent)
        {
            var group = $"inv:{domainEvent.InventoryId}";
            return _hub.Clients.Group(group).SendAsync("chat.messageCreated", domainEvent.Message);
        }
    }
}
