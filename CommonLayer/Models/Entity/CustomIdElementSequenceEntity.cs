using CommonLayer.Enum;

namespace CommonLayer.Models.Entity
{
    public class CustomIdElementSequenceEntity
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public CustomIdElementEnum ElementType { get; set; }
        public int Order { get; set; }
        public string? FixedTextValue { get; set; }
        public int? IncrementValue { get; set; }

        public InventoryItemTypesEntity InventoryItemType { get; set; }
    }
}
