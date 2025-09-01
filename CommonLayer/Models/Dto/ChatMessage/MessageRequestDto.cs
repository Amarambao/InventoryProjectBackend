using CommonLayer.Models.Dto.General;

namespace CommonLayer.Models.Dto.ChatMessage
{
    public class MessageRequestDto : PaginationRequest
    {
        public Guid? UserId { get; set; }
        public IEnumerable<DateTime>? WrittenAt { get; set; }
    }
}
