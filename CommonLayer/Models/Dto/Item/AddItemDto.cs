using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.Item
{
    public class AddItemDto
    {
        public Guid InventoryId { get; set; }
        public string ItemType { get; set; }
        public string? CustomId { get; set; }

        public AddItemDto() { } 

        public AddItemDto(StoredItemsEntity entity, string itemType) 
        { 
            InventoryId = entity.InventoryId;
            ItemType = itemType;
            CustomId = entity.CustomId;
        }
    }
}
