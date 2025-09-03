using CommonLayer.Extensions;
using CommonLayer.Models.Dto.ChatMessage;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class ChatMessagesRepo : IChatMessagesRepo
    {
        private readonly PostgreSQLContext _context;

        public ChatMessagesRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task CreateMessageAsync(ChatMessageEntity message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public Task<List<ChatMessageEntity>> GetChatMessagesWPaginationAsync(MessageRequestDto dto)
        {
            var query = _context.ChatMessages
                .AsNoTracking()
                .Where(c => c.InventoryId == dto.InventoryId!.Value);

            if (!string.IsNullOrWhiteSpace(dto.SearchValue))
                query = query.Where(c => c.Message.CustomNormalize().Contains(dto.SearchValue.CustomNormalize()));

            if (dto.UserId.HasValue)
                query = query.Where(c => c.UserId == dto.UserId.Value);

            if (dto.WrittenAt is not null && dto.WrittenAt.Any())
                query = query.Where(c => dto.WrittenAt.Contains(c.WrittenAt));

            return query
                .OrderByDescending(c => c.WrittenAt)
                .Skip(dto.Page * dto.ReturnCount)
                .Take(dto.ReturnCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUserEntity>> GetChattersAsync(Guid inventoryId)
        {
            var chatterIds = await _context.ChatMessages
                .AsNoTracking()
                .Where(c => c.InventoryId == inventoryId)
                .Select(c => c.UserId)
                .ToListAsync();

            return await _context.Users
                .AsNoTracking()
                .Where(u => chatterIds.Contains(u.Id))
                .ToListAsync();
        }

        public Task<List<ChatMessageEntity>> GetChatMessagesByTimeAsync(IEnumerable<DateTime> dateTimes)
            => _context.ChatMessages
                .AsNoTracking()
                .Where(m => dateTimes.Contains(m.WrittenAt))
                .ToListAsync();

        public async Task RemoveMessageRangeAsync(IEnumerable<ChatMessageEntity> chatMessages)
        {
            _context.RemoveRange(chatMessages);
            await _context.SaveChangesAsync();
        }
    }
}