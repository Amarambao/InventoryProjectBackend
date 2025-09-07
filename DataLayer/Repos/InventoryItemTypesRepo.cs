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

        public async Task UpdateInventoryItemTypesAsync(Guid inventoryId, IEnumerable<InventoryItemTypesEntity> newInventoryItemTypes)
        {
            var inventoryItemTypes = await _context.InventoryItemTypes
                .Where(i => i.InventoryId == inventoryId)
                .ToListAsync();

            _context.InventoryItemTypes.RemoveRange(inventoryItemTypes);

            await _context.InventoryItemTypes.AddRangeAsync(newInventoryItemTypes);

            await _context.SaveChangesAsync();
        }
    }
}
