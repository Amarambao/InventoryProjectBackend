using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IStoredItemsRepo
    {
        Task AddItemAsync(StoredItemsEntity item);
        Task<List<StoredItemsEntity>> GetStoredItemsWPaginationAsync(PaginationRequest dto);
        Task<StoredItemsEntity?> GetFullInfoAsync(Guid itemId);
        Task<bool> IsItemExistAsync(Guid inventoryId, Guid itemId, string customId);
        Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds);
    }
}
