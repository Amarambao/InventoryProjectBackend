using CommonLayer.Enum;
using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class StoredItemsRepo : IStoredItemsRepo
    {
        private readonly PostgreSQLContext _context;

        public StoredItemsRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(StoredItemsEntity item)
        {
            await _context.StoredItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public Task<List<StoredItemsEntity>> GetStoredItemsWPaginationAsync(PaginationRequest dto)
        {
            var query = _context.StoredItems
                .Include(i => i.InventoryItemType)
                    .ThenInclude(i => i.Item)
                .OrderBy(i => i.InventoryItemType.Item.NormalizedName)
                .ThenBy(i => i.CustomId)
                .Where(i => i.InventoryId == dto.InventoryId!.Value);

            if (!string.IsNullOrWhiteSpace(dto.SearchValue))
                query = query.Where(i => i.InventoryItemType.Item.NormalizedName.Contains(dto.SearchValue.CustomNormalize())
                    || i.CustomId.Contains(dto.SearchValue.CustomNormalize()));

            return query
                .Skip(dto.Page * dto.ReturnCount)
                .Take(dto.ReturnCount)
                .ToListAsync();
        }

        public async Task<int?> GetMaxUIntStoredAsync(Guid inventoryId, Guid itemId)
        {
            var uintSequenceIndex = (await _context.CustomIdSequence
                .Where(e => e.InventoryId == inventoryId && e.ItemId == itemId)
                .OrderBy(e => e.Order)
                .ToListAsync())
                .FindIndex(e => e.ElementType == CustomIdElementEnum.UIntSequence);

            if (uintSequenceIndex == -1)
                return null;

            var storedCustomIds = await _context.StoredItems
                .Where(s => s.InventoryId == inventoryId && s.ItemId == itemId)
                .Select(s => s.CustomId)
                .ToListAsync();

            var maxSequence = storedCustomIds
                .Select(id => id.Split('-'))
                .Where(parts => parts.Length > uintSequenceIndex)
                .Select(parts => int.TryParse(parts[uintSequenceIndex], out var val) ? val : 0)
                .DefaultIfEmpty(0)
                .Max();

            return maxSequence + 1;
        }

        public Task<bool> IsItemExistAsync(Guid inventoryId, Guid itemId, string customId)
            => _context.StoredItems
                .Where(i => i.InventoryId == inventoryId && i.ItemId == itemId)
                .AnyAsync(i => i.CustomId == customId);

        public async Task UpdateRangeAsync(IEnumerable<StoredItemsEntity> storedItems)
        {
            _context.UpdateRange(storedItems);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds)
        {
            var storedItems = await _context.StoredItems.Where(si => storedItemIds.Contains(si.Id)).ToListAsync();

            _context.RemoveRange(storedItems);
            await _context.SaveChangesAsync();
        }
    }
}
