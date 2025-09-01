using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryEditorsRepo
    {
        Task CreateRangeAsync(IEnumerable<InventoryEditorsEntity> userInventories);
        Task<List<InventoryEditorsEntity>> GetInventoryEditorsAsync(Guid inventoryId, IEnumerable<Guid>? userIds = null);
        Task RemoveRangeAsync(IEnumerable<InventoryEditorsEntity> userInventories);
    }
}
