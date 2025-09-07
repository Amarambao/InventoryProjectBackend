using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Dto.Message;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using SignalR;

namespace BusinessLayer.Services
{
    public class ChatMessagesSrv : IChatMessagesSrv
    {
        private readonly IChatMessagesRepo _chatMessagesRepo;
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly IEventDispatcher _dispatcher;

        public ChatMessagesSrv(
            IChatMessagesRepo chatMessagesRepo,
            UserManager<AppUserEntity> userManager,
            IEventDispatcher dispatcher)
        {
            _chatMessagesRepo = chatMessagesRepo;
            _userManager = userManager;
            _dispatcher = dispatcher;
        }

        public async Task CreateMessageAsync(MessagePostDto dto, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return;

            var userName = string.Empty;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is not null)
                userName = user.UserName;

            var messageEntity = new ChatMessageEntity(dto, userId, userName);

            await _chatMessagesRepo.CreateMessageAsync(messageEntity);

            await _dispatcher.DispatchAsync(new ChatMessageCreatedEvent(dto.InventoryId, new MessageGetDto(messageEntity)));
        }

        public async Task<IEnumerable<MessageGetDto>> GetChatMessagesWPaginationAsync(MessageRequestDto dto)
            => (await _chatMessagesRepo.GetChatMessagesWPaginationAsync(dto)).Select(m => new MessageGetDto(m));

        public async Task RemoveMessageRangeAsync(IEnumerable<DateTime> dateTimes)
        {
            var messages = await _chatMessagesRepo.GetChatMessagesByTimeAsync(dateTimes);

            await _chatMessagesRepo.RemoveMessageRangeAsync(messages);
        }
    }
}
