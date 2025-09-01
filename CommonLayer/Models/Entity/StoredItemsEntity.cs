namespace CommonLayer.Models.Entity
{
    public class StoredItemsEntity
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public string CustomId { get; set; } = string.Empty;

        public InventoryItemTypesEntity InventoryItemType { get; set; }
    }
}
