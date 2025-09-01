namespace CommonLayer.Models.Dto.General
{
    public class PaginationRequest
    {
        public int Page { get; set; }
        public int ReturnCount { get; set; }
        public Guid? InventoryId { get; set; }
        public string? SearchValue { get; set; }
    }
}
