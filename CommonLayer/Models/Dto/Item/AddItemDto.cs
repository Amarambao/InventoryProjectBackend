using CommonLayer.Models.Dto.CustomDescription;
using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.Item
{
    public class AddItemDto
    {
        public Guid InventoryId { get; set; }
        public Guid ItemTypeId { get; set; }
        public string? CustomId { get; set; }
        public List<ItemDescriptionElementDto> ItemDescription { get; set; }

        public AddItemDto() { }

        public AddItemDto(StoredItemsEntity entity, Guid itemTypeId)
        {
            InventoryId = entity.InventoryId;
            ItemTypeId = itemTypeId;
            CustomId = entity.CustomId;
            ItemDescription = entity.StoredItemDescriptions
                .OrderBy(sid => sid.Order)
                .Select(sid => new ItemDescriptionElementDto(sid))
                .ToList();
        }
    }
}
