using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Item;

namespace BusinessLayer.Interfaces
{
    public interface IStoredItemsSrv
    {
        Task<AddItemDto?> AddItemAsync(AddItemDto dto);
        Task<IEnumerable<StoredItemGetAllDto>> GetAllWPagination(PaginationRequest dto);
        Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds);
    }
}
