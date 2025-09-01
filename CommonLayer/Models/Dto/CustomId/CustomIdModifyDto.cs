namespace CommonLayer.Models.Dto.CustomId
{
    public class CustomIdModifyDto
    {
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public List<CustomIdElementDto> Sequence { get; set; }
    }
}
