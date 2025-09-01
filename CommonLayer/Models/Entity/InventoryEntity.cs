using CommonLayer.Models.Dto.Inventory;

namespace CommonLayer.Models.Entity
{
    public class InventoryEntity
    {
        public Guid Id { get; set; }
        public Guid InventoryTypeId { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }

        public InventoryTypeEntity InventoryType { get; set; }

        public List<InventoryEditorsEntity> InventoryEditors { get; set; }
        public List<InventoryItemTypesEntity> InventoryItemTypes { get; set; }
        public List<InventoryTagEntity> InventoryTags { get; set; }
        public List<ChatMessageEntity> ChatMessages { get; set; }

        public InventoryEntity() { }

        public InventoryEntity(InventoryCreateDto dto, Guid creatorId, Guid inventoryTypeId)
        {
            Guid newId = Guid.NewGuid();

            Id = newId;
            InventoryTypeId = inventoryTypeId;
            Description = dto.Description;
            IsPublic = dto.IsPublic;
            ChatMessages = [];
            InventoryEditors = [new() { InventoryId = Id, UserId = creatorId, IsCreator = true }];
        }
    }
}
