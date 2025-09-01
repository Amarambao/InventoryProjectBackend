using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryTagRepo
    {
        Task CreateRangeAsync(IEnumerable<InventoryTagEntity> inventoryTags);
        Task<List<InventoryTagEntity>> GetRangeAsync(Guid inventoryId);
        Task RemoveRangeAsync(IEnumerable<InventoryTagEntity> inventoryTags);
    }
}
