using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Inventory;

namespace BusinessLayer.Interfaces
{
    public interface IInventorySrv
    {
        Task<Guid> CreateInventoryAsync(InventoryCreateDto dto, Guid creatorId);
        Task<IEnumerable<InventoryGetLiteDto>> GetInventoriesLiteAsync(InventoryRequestDto dto);
        Task<ResultDto<InventoryGetFullDto>> GetInventoryFullAsync(Guid inventoryId);
        Task<ResultDto> UpdateInventoryAsync(InventoryUpdateDto dto);
        Task RemoveInventoryRangeAsync(IEnumerable<Guid> inventoryIds);
    }
}
