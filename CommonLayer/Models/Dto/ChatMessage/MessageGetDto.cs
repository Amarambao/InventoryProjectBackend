using CommonLayer.Models.Entity;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Dto.Message
{
    public class MessageGetDto
    {
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required DateTime WrittenAt { get; set; }
        public required string Message { get; set; }

        public MessageGetDto() { }

        [SetsRequiredMembers]
        public MessageGetDto(ChatMessageEntity message, string userName)
        {
            UserId = message.UserId;
            UserName = userName; 
            WrittenAt = message.WrittenAt;
            Message = message.Message;
        }
    }
}
