using CommonLayer.Models.Entity;
using DataLayer.Interfaces.Generic;

namespace DataLayer.Interfaces
{
    public interface IInventoryTypeRepo : IGenericIdAndNameRepo<InventoryTypeEntity>
    {
        Task<InventoryTypeEntity> CreateAsync(InventoryTypeEntity entity);
    }
}
