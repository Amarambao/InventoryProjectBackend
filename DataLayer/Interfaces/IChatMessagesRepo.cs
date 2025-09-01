using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IChatMessagesRepo
    {
        Task CreateMessageAsync(ChatMessageEntity message);
        Task<List<ChatMessageEntity>> GetChatMessagesWPaginationAsync(MessageRequestDto dto);
        Task<IEnumerable<AppUserEntity>> GetChattersAsync(Guid inventoryId);
        Task<List<ChatMessageEntity>> GetChatMessagesByTimeAsync(IEnumerable<DateTime> dateTimes);
        Task RemoveMessageRangeAsync(IEnumerable<ChatMessageEntity> chatMessages);
    }
}
