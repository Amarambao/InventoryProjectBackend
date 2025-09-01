namespace CommonLayer.Models.Dto.Message
{
    public class MessagePostDto
    {
        public Guid InventoryId { get; set; }
        public required string Message { get; set; }
    }
}
