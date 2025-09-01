using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class CheckRepo : ICheckRepo
    {
        private readonly PostgreSQLContext _context;

        public CheckRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public Task<List<Guid>> GetWhereInventoryCreatorAsync(Guid userId, IEnumerable<Guid> inventoryIds)
            => _context.Inventories
                .Where(i => inventoryIds.Contains(i.Id) && i.InventoryEditors.First(ui => ui.IsCreator).UserId == userId)
                .Select(i => i.Id)
                .ToListAsync();

        public Task<bool> GetWhereInventoryEditorAsync(Guid userId, Guid inventoryId)
            => _context.Inventories
                .AnyAsync(i => (i.Id == inventoryId && i.InventoryEditors.Select(ui => ui.UserId).Contains(userId)) || i.IsPublic);
    }
}
