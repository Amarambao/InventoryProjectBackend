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

        public async Task CreateSequenceAsync(IEnumerable<CustomIdElementSequenceEntity> sequence)
        {
            await _context.CustomIdSequence.AddRangeAsync(sequence);
            await _context.SaveChangesAsync();
        }

        public Task<List<CustomIdElementSequenceEntity>> GetItemSequenceAsync(Guid inventoryId, Guid itemId)
            => _context.CustomIdSequence
                .OrderBy(i => i.Order)
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .ToListAsync();

        public async Task UpdateRangeAsync(IEnumerable<CustomIdElementSequenceEntity> storedItems)
        {
            _context.UpdateRange(storedItems);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<CustomIdElementSequenceEntity> storedItems)
        {
            _context.RemoveRange(storedItems);
            await _context.SaveChangesAsync();
        }
    }
}
