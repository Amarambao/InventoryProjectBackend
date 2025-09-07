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

        public async Task UpdateInventoryTagsAsync(Guid inventoryId, IEnumerable<InventoryTagEntity> newInventoryTags)
        {
            var inventoryTags = await _context.InventoryTags
                .Where(i => i.InventoryId == inventoryId)
                .ToListAsync();

            _context.InventoryTags.RemoveRange(inventoryTags);

            await _context.InventoryTags.AddRangeAsync(newInventoryTags);

            await _context.SaveChangesAsync();
        }
    }
}
