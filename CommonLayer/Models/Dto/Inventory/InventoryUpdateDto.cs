namespace CommonLayer.Models.Dto.Inventory
{
    public class InventoryUpdateDto
    {
        public Guid InventoryId { get; set; }
        public bool IsPublic { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
