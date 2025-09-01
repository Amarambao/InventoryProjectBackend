namespace CommonLayer.Models.Entity
{
    public class InventoryTagEntity
    {
        public Guid InventoryId { get; set; }
        public Guid TagId { get; set; }

        public InventoryEntity Inventory { get; set; }
        public TagEntity Tag { get; set; }
    }
}
