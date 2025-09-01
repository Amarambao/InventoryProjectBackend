namespace CommonLayer.Models.Dto.Inventory
{
    public class InventoryCreateDto
    {
        public required string InventoryType { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public List<string> ItemNames { get; set; }
        public List<string> Tags { get; set; }
    }
}
