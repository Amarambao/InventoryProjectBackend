using CommonLayer.Models.Dto.Message;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity
{
    public class ChatMessageEntity
    {
        public Guid InventoryId { get; set; }
        public required Guid UserId { get; set; }
        public required DateTime WrittenAt { get; set; }
        public required string Message { get; set; }

        public InventoryEntity Inventory { get; set; }

        public ChatMessageEntity() { }

        [SetsRequiredMembers]
        public ChatMessageEntity(MessagePostDto dto, Guid userId)
        {
            InventoryId = dto.InventoryId;
            UserId = userId;
            WrittenAt = DateTime.UtcNow;
            Message = dto.Message;
        }
    }
}
