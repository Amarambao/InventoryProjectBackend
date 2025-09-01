using CommonLayer.Models.Entity;
using DataLayer.Interfaces.Generic;

namespace DataLayer.Interfaces
{
    public interface IItemTypeRepo : IGenericIdAndNameRepo<ItemTypeEntity>
    {
        Task<List<ItemTypeEntity>> GetItemRangeAsync(Guid? inventoryId = null, IEnumerable<string>? itemTypes = null);
    }
}
