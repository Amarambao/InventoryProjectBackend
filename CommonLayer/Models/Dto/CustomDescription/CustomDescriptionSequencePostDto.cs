namespace CommonLayer.Models.Dto.CustomDescription
{
    public class CustomDescriptionSequencePostDto
    {
        public Guid InventoryId { get; set; }
        public Guid ItemId { get; set; }
        public List<DescriptionElementDto> Sequence { get; set; }
    }
}
