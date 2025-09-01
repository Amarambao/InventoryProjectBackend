using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class InventoryEditorsRepo : IInventoryEditorsRepo
    {
        private readonly PostgreSQLContext _context;

        public InventoryEditorsRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(IEnumerable<InventoryEditorsEntity> userInventories)
        {
            await _context.InventoryEditors.AddRangeAsync(userInventories);
            await _context.SaveChangesAsync();
        }

        public Task<List<InventoryEditorsEntity>> GetInventoryEditorsAsync(Guid inventoryId, IEnumerable<Guid>? userIds = null)
        {
            var query = _context.InventoryEditors
                .AsNoTracking()
                .Include(ui => ui.User)
                .Where(ui => ui.InventoryId == inventoryId);

            if (userIds is not null && userIds.Any())
                query = query.Where(ui => userIds.Contains(ui.UserId));

            return query.ToListAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<InventoryEditorsEntity> userInventories)
        {
            _context.InventoryEditors.RemoveRange(userInventories);
            await _context.SaveChangesAsync();
        }
    }
}
