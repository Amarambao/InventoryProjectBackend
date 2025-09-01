using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Dto.Message;

namespace BusinessLayer.Interfaces
{
    public interface IChatMessagesSrv
    {
        Task CreateMessageAsync(MessagePostDto dto, Guid userId);
        Task<IEnumerable<MessageGetDto>> GetChatMessagesWPaginationAsync(MessageRequestDto dto);
        Task RemoveMessageRangeAsync(IEnumerable<DateTime> dateTimes);
    }
}
