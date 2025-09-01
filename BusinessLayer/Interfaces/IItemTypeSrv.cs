using BusinessLayer.Interfaces.Generic;
using CommonLayer.Models.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IItemTypeSrv : IGenericIdAndNameService<ItemTypeEntity>
    {
        Task<List<ItemTypeEntity>> GetItemRangeAsync(Guid? inventoryId = null, IEnumerable<string>? itemTypes = null);
    }
}
