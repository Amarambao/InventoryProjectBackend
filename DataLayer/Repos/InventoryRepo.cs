using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Inventory;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly PostgreSQLContext _context;

        public InventoryRepo(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(InventoryEntity entity)
        {
            await _context.Inventories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<List<InventoryEntity>> GetRangeWPaginationAsync(InventoryRequestDto dto)
        {
            var query = _context.Inventories
                .AsNoTracking()
                .Include(i => i.InventoryType)
                .Include(i => i.InventoryEditors)
                    .ThenInclude(ui => ui.User)
                .Include(i => i.InventoryItemTypes)
                    .ThenInclude(i => i.StoredItems)
                .OrderByDescending(i => i.InventoryItemTypes.SelectMany(iit => iit.StoredItems).Count())
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.SearchValue))
                query = query.Where(i => i.InventoryType.NormalizedName.Contains(dto.SearchValue.CustomNormalize())
                    || i.InventoryTags.Select(t => t.Tag.NormalizedName).Contains(dto.SearchValue.CustomNormalize()));

            if (dto.UserId.HasValue)
                query = query.Where(i => i.InventoryEditors.Any(ui => ui.UserId == dto.UserId.Value && ui.IsCreator == (dto.IsCreator ?? false)));

            return query
                .Skip(dto.Page * dto.ReturnCount)
                .Take(dto.ReturnCount)
                .ToListAsync();
        }

        public Task<InventoryEntity?> FindByIdAsync(Guid itemId)
            => _context.Inventories
            .AsNoTracking()
            .Include(x => x.InventoryType)
            .Include(x => x.InventoryTags)
                .ThenInclude(y => y.Tag)
            .Include(x => x.InventoryEditors)
                .ThenInclude(y => y.User)
            .FirstOrDefaultAsync(x => x.Id == itemId);

        public async Task<ResultDto?> UpdateAsync(InventoryUpdateDto dto)
        {
            var inventory = await _context.Inventories
                .Include(i => i.InventoryType)
                .FirstOrDefaultAsync(i => i.Id == dto.InventoryId);

            if (inventory is null)
                return new(false, "Inventory not found");

            if (inventory.IsPublic != dto.IsPublic)
            {
                inventory.IsPublic = dto.IsPublic;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                inventory.InventoryType.Name = dto.Name;
                inventory.InventoryType.NormalizedName = dto.Name.CustomNormalize();
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                inventory.Description = dto.Description;
            }

            await _context.SaveChangesAsync();
            return null;
        }

        public async Task RemoveRangeAsync(IEnumerable<Guid> inventoryIds)
        {
            var inventories = await _context.Inventories.Where(i => inventoryIds.Contains(i.Id)).ToListAsync();

            _context.Inventories.RemoveRange(inventories);
            await _context.SaveChangesAsync();
        }
    }
}
