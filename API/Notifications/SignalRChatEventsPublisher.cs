using API.Hubs;
using CommonLayer.Models.Dto.Message;
using Microsoft.AspNetCore.SignalR;

namespace API.Notifications
{
    public class SignalRChatEventsPublisher : IChatEventsPublisher
    {
        private readonly IHubContext<ChatHub> _hub;

        public SignalRChatEventsPublisher(IHubContext<ChatHub> hub) => _hub = hub;

        private static string GroupName(Guid inventoryId) => $"inv:{inventoryId}";

        public Task MessageCreatedAsync(Guid inventoryId, MessageGetDto dto)
            => _hub.Clients.Group(GroupName(inventoryId)).SendAsync("chat.messageCreated", dto);

        public Task MessagesDeletedAsync(Guid inventoryId, IEnumerable<DateTime> writtenAtsUtc)
            => _hub.Clients.Group(GroupName(inventoryId)).SendAsync("chat.messagesDeleted", writtenAtsUtc);
    }
}
