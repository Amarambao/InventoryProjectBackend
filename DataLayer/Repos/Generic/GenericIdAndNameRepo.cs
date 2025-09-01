using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity.General;
using DataLayer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repos.Generic
{
    public abstract class GenericIdAndNameRepo<TEntity> where TEntity : IdAndName
    {
        protected readonly PostgreSQLContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected GenericIdAndNameRepo(PostgreSQLContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual Task<TEntity?> FindByNameAsync(string search)
            => _dbSet.FirstOrDefaultAsync(x => x.NormalizedName == search.CustomNormalize());

        public virtual Task<List<string>> GetNamesWithPaginationAsync(PaginationRequest dto)
        {
            var query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(dto.SearchValue))
                query = query.Where(e => e.NormalizedName.Contains(dto.SearchValue.CustomNormalize()));

            return query
                .OrderBy(e => e.NormalizedName)
                .Skip(dto.Page * dto.ReturnCount)
                .Take(dto.ReturnCount)
                .Select(e => e.Name)
                .ToListAsync();
        }

        public virtual async Task<List<string>> GetNonExistingNamesAsync(IEnumerable<string> names)
        {
            var normalized = names.Select(x => x.CustomNormalize()).ToList();

            var existingNames = await _dbSet
                .AsNoTracking()
                .Where(e => normalized.Contains(e.NormalizedName))
                .Select(e => e.NormalizedName)
                .ToHashSetAsync();

            return normalized
                .Where(name => !existingNames.Contains(name))
                .ToList();
        }
    }
}
