using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class InventoryTagRepo : IInventoryTagRepo
    {
        private readonly PostgreSQLContext _context;

        public InventoryTagRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(IEnumerable<InventoryTagEntity> inventoryTags)
        {
            await _context.InventoryTags.AddRangeAsync(inventoryTags);
            await _context.SaveChangesAsync();
        }

        public Task<List<InventoryTagEntity>> GetRangeAsync(Guid inventoryId)
            => _context.InventoryTags
                .AsNoTracking()
                .Where(it => it.InventoryId == inventoryId)
                .ToListAsync();

        public async Task RemoveRangeAsync(IEnumerable<InventoryTagEntity> inventoryTags)
        {
            _context.InventoryTags.RemoveRange(inventoryTags);
            await _context.SaveChangesAsync();
        }
    }
}
