namespace CommonLayer.Models.Dto.Item
{
    public class StoredItemGetAllDto
    {
        public string ItemName { get; set; }
        public IEnumerable<StoredItemDto> ItemIds { get; set; }
    }
}
