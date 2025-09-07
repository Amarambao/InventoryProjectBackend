using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryItemTypesRepo
    {
        Task UpdateInventoryItemTypesAsync(Guid inventoryId, IEnumerable<InventoryItemTypesEntity> newInventoryItemTypes);
    }
}
