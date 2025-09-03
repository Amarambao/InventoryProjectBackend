using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class InventoryItemTypesRepo : IInventoryItemTypesRepo
    {
        private readonly PostgreSQLContext _context;

        public InventoryItemTypesRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(IEnumerable<InventoryItemTypesEntity> inventoryItemTypes)
        {
            await _context.InventoryItemTypes.AddRangeAsync(inventoryItemTypes);
            await _context.SaveChangesAsync();
        }

        public Task<List<InventoryItemTypesEntity>> GetRangeAsync(Guid inventoryId)
            => _context.InventoryItemTypes
                .AsNoTracking()
                .Include(ii => ii.Item)
                .Where(ii => ii.InventoryId == inventoryId)
                .ToListAsync();

        public async Task RemoveRangeAsync(IEnumerable<InventoryItemTypesEntity> inventoryItemTypes)
        {
            _context.InventoryItemTypes.RemoveRange(inventoryItemTypes);
            await _context.SaveChangesAsync();
        }
    }
}
