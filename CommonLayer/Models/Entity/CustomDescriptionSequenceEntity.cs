using CommonLayer.Enum;

namespace CommonLayer.Models.Entity
{
    public class CustomDescriptionSequenceEntity
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public CustomDescriptionFieldEnum DescripionType { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public InventoryItemTypesEntity InventoryItemType { get; set; }
    }
}
