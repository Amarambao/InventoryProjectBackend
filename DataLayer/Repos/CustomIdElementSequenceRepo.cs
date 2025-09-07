using CommonLayer.Enum;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class CustomIdElementSequenceRepo : ICustomIdElementSequenceRepo
    {
        private readonly PostgreSQLContext _context;

        public CustomIdElementSequenceRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task UpdateSequenceAsync(Guid inventoryId, Guid itemId, IEnumerable<CustomIdElementSequenceEntity> newSequence)
        {
            var sequence = await _context.CustomIdSequence
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .ToListAsync();

            _context.CustomIdSequence.RemoveRange(sequence);

            await _context.CustomIdSequence.AddRangeAsync(newSequence);

            await _context.SaveChangesAsync();
        }

        public Task<List<CustomIdElementSequenceEntity>> GetItemSequenceAsync(Guid inventoryId, Guid itemId)
            => _context.CustomIdSequence
                .AsNoTracking()
                .OrderBy(i => i.Order)
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .ToListAsync();

        public Task<CustomIdElementSequenceEntity?> GetMaxUIntElementStoredAsync(Guid inventoryId, Guid itemId)
            => _context.CustomIdSequence
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.InventoryId == inventoryId && s.ItemId == itemId && s.ElementType == CustomIdElementEnum.UIntSequence);

        public async Task UpdateRangeAsync(IEnumerable<CustomIdElementSequenceEntity> customIdSequence)
        {
            _context.CustomIdSequence.UpdateRange(customIdSequence);
            await _context.SaveChangesAsync();
        }
    }
}
