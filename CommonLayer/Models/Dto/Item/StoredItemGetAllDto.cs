namespace CommonLayer.Models.Dto.Item
{
    public class StoredItemGetAllDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public IEnumerable<StoredItemGetLiteDto> StoredItemsId { get; set; }
    }
}
