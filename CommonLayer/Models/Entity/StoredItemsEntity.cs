using CommonLayer.Models.Dto.Item;

namespace CommonLayer.Models.Entity
{
    public class StoredItemsEntity
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomId { get; set; } = string.Empty;

        public InventoryItemTypesEntity InventoryItemType { get; set; }

        public List<StoredItemDescriptionEntity> StoredItemDescriptions { get; set; }

        public StoredItemsEntity() { }

        public StoredItemsEntity(AddItemDto dto, AppUserEntity creator, Guid itemId, string customId)
        {
            Id = Guid.NewGuid();
            InventoryId = dto.InventoryId;
            ItemId = ItemId;
            CreatorId = creator.Id;
            CreatorName = creator.UserName ?? string.Empty;
            CreatedAt = DateTime.UtcNow;
            CustomId = customId;
        }
    }
}
