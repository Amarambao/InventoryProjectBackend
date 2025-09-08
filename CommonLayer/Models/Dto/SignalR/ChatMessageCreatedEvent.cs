using CommonLayer.Models.Dto.Message;

namespace SignalR
{
    public class ChatMessageCreatedEvent
    {
        public Guid InventoryId { get; }
        public MessageGetDto Message { get; }

        public ChatMessageCreatedEvent(Guid inventoryId, MessageGetDto message)
        {
            InventoryId = inventoryId;
            Message = message;
        }
    }
}
