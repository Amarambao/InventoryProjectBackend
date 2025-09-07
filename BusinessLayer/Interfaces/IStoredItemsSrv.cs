using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Item;

namespace BusinessLayer.Interfaces
{
    public interface IStoredItemsSrv
    {
        Task<ResultDto<AddItemDto?>> AddItemAsync(AddItemDto dto, Guid creatorId);
        Task<IEnumerable<StoredItemGetAllDto>> GetAllWPaginationAsync(PaginationRequest dto);
        Task<StoredItemGetFullDto?> GetFullInfoAsync(Guid itemId);
        Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds);
    }
}
