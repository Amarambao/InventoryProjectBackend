namespace CommonLayer.Models.Entity
{
    public class InventoryEditorsEntity
    {
        public required Guid InventoryId { get; set; }
        public required Guid UserId { get; set; }
        public bool IsCreator { get; set; }

        public AppUserEntity User { get; set; }
        public InventoryEntity Inventory { get; set; }
    }
}
