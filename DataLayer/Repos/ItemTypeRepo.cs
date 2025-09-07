using CommonLayer.Extensions;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using DataLayer.Repos.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class ItemTypeRepo : GenericIdAndNameRepo<ItemTypeEntity>, IItemTypeRepo
    {
        public ItemTypeRepo(PostgreSQLContext context) : base(context, context.ItemType) { }

        public Task<List<ItemTypeEntity>> GetItemRangeAsync(Guid? inventoryId = null, IEnumerable<string>? itemTypes = null)
        {
            var query = _dbSet.AsNoTracking();

            if (inventoryId.HasValue)
                query = query.Where(x => x.InventoryItemTypes!.Any(x => x.InventoryId == inventoryId));

            if (itemTypes is not null && itemTypes.Any())
                query = query.Where(x => itemTypes.Select(x => x.CustomNormalize()).ToHashSet().Contains(x.NormalizedName));

            return query.ToListAsync();
        }
    }
}
