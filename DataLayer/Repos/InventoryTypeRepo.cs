using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using DataLayer.Repos.Generic;

namespace DataLayer.Repos
{
    public class InventoryTypeRepo : GenericIdAndNameRepo<InventoryTypeEntity>, IInventoryTypeRepo
    {
        public InventoryTypeRepo(PostgreSQLContext context) : base(context, context.InventoryType) { }

        public async Task<InventoryTypeEntity> CreateAsync(InventoryTypeEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
