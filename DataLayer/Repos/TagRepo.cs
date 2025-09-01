using CommonLayer.Extensions;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using DataLayer.Repos.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos
{
    public class TagRepo : GenericIdAndNameRepo<TagEntity>, ITagRepo
    {
        public TagRepo(PostgreSQLContext context) : base(context, context.Tags) { }

        public Task<List<TagEntity>> GetTagRangeAsync(Guid? inventoryId = null, IEnumerable<string>? tags = null)
        {
            var query = _dbSet.AsNoTracking();

            if (inventoryId.HasValue)
                query = query.Where(x => x.InventoryTags!.Any(x => x.InventoryId == inventoryId));

            if (tags is not null && tags.Any())
                query = query.Where(x => tags.Select(x => x.CustomNormalize()).Contains(x.NormalizedName));

            return query.ToListAsync();
        }
    }
}
