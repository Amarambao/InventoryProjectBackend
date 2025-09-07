using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class CustomDescriptionSequenceRepo : ICustomDescriptionSequenceRepo
    {
        private readonly PostgreSQLContext _context;

        public CustomDescriptionSequenceRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task UpdateSequenceAsync(Guid inventoryId, Guid itemId, IEnumerable<CustomDescriptionSequenceEntity> newSequence)
        {
            var sequence = await _context.CustomDescriptionSequence
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .ToListAsync();

            _context.CustomDescriptionSequence.RemoveRange(sequence);

            await _context.CustomDescriptionSequence.AddRangeAsync(newSequence);

            await _context.SaveChangesAsync();
        }

        public Task<List<CustomDescriptionSequenceEntity>> GetSequenceAsync(Guid inventoryId, Guid itemId)
            => _context.CustomDescriptionSequence
                .AsNoTracking()
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .OrderBy(i => i.Order)
                .ToListAsync();
    }
}
