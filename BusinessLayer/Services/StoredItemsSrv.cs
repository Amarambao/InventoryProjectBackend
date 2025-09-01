using BusinessLayer.Interfaces;
using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Item;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class StoredItemsSrv : IStoredItemsSrv
    {
        private readonly IStoredItemsRepo _storedItemsRepo;
        private readonly ICustomIdElementSequenceSrv _customIdElementSequenceSrv;
        private readonly IItemTypeSrv _itemTypeSrv;

        public StoredItemsSrv(
            IStoredItemsRepo storedItemsRepo,
            ICustomIdElementSequenceSrv customIdElementSequenceSrv,
            IItemTypeSrv itemTypeSrv)
        {
            _storedItemsRepo = storedItemsRepo;
            _customIdElementSequenceSrv = customIdElementSequenceSrv;
            _itemTypeSrv = itemTypeSrv;
        }

        public async Task<AddItemDto?> AddItemAsync(AddItemDto dto)
        {
            var requestedItem = (await _itemTypeSrv.GetItemRangeAsync(dto.InventoryId)).FirstOrDefault(i => i.NormalizedName == dto.ItemType.CustomNormalize());

            if (requestedItem is null)
                return null;

            var item = new StoredItemsEntity()
            {
                Id = Guid.NewGuid(),
                InventoryId = dto.InventoryId,
                ItemId = requestedItem.Id,
                CustomId = dto.CustomId ?? await _customIdElementSequenceSrv.GenerateCustomIdAsync(dto.InventoryId, requestedItem.Id)
            };

            if (await _storedItemsRepo.IsItemExistAsync(dto.InventoryId, requestedItem.Id, item.CustomId))
                return new(item, dto.ItemType);

            await _storedItemsRepo.AddItemAsync(item);

            return null;
        }

        public async Task<IEnumerable<StoredItemGetAllDto>> GetAllWPagination(PaginationRequest dto)
            => (await _storedItemsRepo.GetStoredItemsWPaginationAsync(dto))
            .GroupBy(i => i.InventoryItemType.Item.Name)
            .Select(g => new StoredItemGetAllDto
            {
                ItemName = g.Key,
                ItemIds = g.Select(x => new StoredItemDto
                {
                    Id = x.Id,
                    CustomId = x.CustomId
                })
            });

        public Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds)
            => _storedItemsRepo.RemoveRangeAsync(storedItemIds);
    }
}
