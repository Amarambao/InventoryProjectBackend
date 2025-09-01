using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryItemTypesRepo
    {
        Task CreateRangeAsync(IEnumerable<InventoryItemTypesEntity> inventoryItems);
        Task<List<InventoryItemTypesEntity>> GetRangeAsync(Guid inventoryId);
        Task RemoveRangeAsync(IEnumerable<InventoryItemTypesEntity> inventoryItems);
    }
}
