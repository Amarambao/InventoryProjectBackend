using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Inventory;
using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IInventoryRepo
    {
        Task CreateAsync(InventoryEntity entity);
        Task<List<InventoryEntity>> GetRangeWPaginationAsync(InventoryRequestDto dto);
        Task<InventoryEntity?> FindByIdAsync(Guid itemId);
        Task<ResultDto?> UpdateAsync(InventoryUpdateDto dto);
        Task RemoveRangeAsync(IEnumerable<Guid> inventoryIds);
    }
}
