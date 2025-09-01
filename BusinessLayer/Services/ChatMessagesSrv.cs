using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Dto.Message;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ChatMessagesSrv : IChatMessagesSrv
    {
        private readonly IChatMessagesRepo _chatMessagesRepo;

        public ChatMessagesSrv(IChatMessagesRepo chatMessagesRepo)
        {
            _chatMessagesRepo = chatMessagesRepo;
        }

        public async Task CreateMessageAsync(MessagePostDto dto, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return;

            await _chatMessagesRepo.CreateMessageAsync(new ChatMessageEntity(dto, userId));
        }

        public async Task<IEnumerable<MessageGetDto>> GetChatMessagesWPaginationAsync(MessageRequestDto dto)
        {
            var messages = await _chatMessagesRepo.GetChatMessagesWPaginationAsync(dto);

            var chatters = await _chatMessagesRepo.GetChattersAsync(dto.InventoryId!.Value);

            return messages.Select(m =>
            {
                var userName = chatters.FirstOrDefault(u => u.Id == m.UserId)?.UserName ?? "Unknown";
                return new MessageGetDto(m, userName);
            });
        }

        public async Task RemoveMessageRangeAsync(IEnumerable<DateTime> dateTimes)
        {
            var messages = await _chatMessagesRepo.GetChatMessagesByTimeAsync(dateTimes);

            await _chatMessagesRepo.RemoveMessageRangeAsync(messages);
        }
    }
}
