using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryTagRepo
    {
        Task UpdateInventoryTagsAsync(Guid inventoryId, IEnumerable<InventoryTagEntity> newInventoryTags);
    }
}
