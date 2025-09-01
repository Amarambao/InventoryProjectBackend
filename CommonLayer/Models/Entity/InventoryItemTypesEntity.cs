namespace CommonLayer.Models.Entity
{
    public class InventoryItemTypesEntity
    {
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }

        public InventoryEntity Inventory { get; set; }
        public ItemTypeEntity Item { get; set; }
        public List<CustomIdElementSequenceEntity> CustomIdSequence { get; set; }
        public List<StoredItemsEntity> StoredItems { get; set; }
    }
}
