using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Inventory;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class InventorySrv : IInventorySrv
    {
        private readonly IInventoryRepo _invRepo;
        private readonly IInventoryTypeSrv _invTypeSrv;
        private readonly IInventoryItemTypesSrv _invItemSrv;
        private readonly IItemTypeSrv _itemTypeSrv;
        private readonly ITagSrv _tagSrv;
        private readonly IInventoryTagSrv _invTagSrv;

        public InventorySrv(
            IInventoryRepo invRepo,
            IInventoryTypeSrv invTypeSrv,
            IInventoryItemTypesSrv invItemSrv,
            IItemTypeSrv itemTypeSrv,
            ITagSrv tagSrv,
            IInventoryTagSrv invTagSrv)
        {
            _invRepo = invRepo;
            _invTypeSrv = invTypeSrv;
            _invItemSrv = invItemSrv;
            _itemTypeSrv = itemTypeSrv;
            _tagSrv = tagSrv;
            _invTagSrv = invTagSrv;
        }

        public async Task<Guid> CreateInventoryAsync(InventoryCreateDto dto, Guid creatorId)
        {
            var inventoryTypeId = await _invTypeSrv.ManageCreationAsync(dto.InventoryType);

            var inventoryEntity = new InventoryEntity(dto, creatorId, inventoryTypeId);

            await _invRepo.CreateAsync(inventoryEntity);

            await _itemTypeSrv.CreateNonExistingAsync(dto.ItemNames);

            await _invItemSrv.ModifyInventoryItemsRangeAsync(inventoryEntity.Id, dto.ItemNames);

            await _tagSrv.CreateNonExistingAsync(dto.Tags);

            await _invTagSrv.ModifyInventoryTagsRangeAsync(inventoryEntity.Id, dto.Tags);

            return inventoryEntity.Id;
        }

        public async Task<IEnumerable<InventoryGetLiteDto>> GetInventoriesLiteAsync(InventoryRequestDto dto)
            => (await _invRepo.GetRangeWPaginationAsync(dto)).Select(i => new InventoryGetLiteDto(i));

        public async Task<ResultDto<InventoryGetFullDto>> GetInventoryFullAsync(Guid inventoryId)
        {
            var inventory = await _invRepo.FindByIdAsync(inventoryId);

            if (inventory is null)
                return new(false, "Inventory not found");

            return new(true, null, new(inventory));
        }

        public async Task<ResultDto?> UpdateInventoryAsync(InventoryUpdateDto dto)
            => await _invRepo.UpdateAsync(dto);

        public Task RemoveInventoryRangeAsync(IEnumerable<Guid> inventoryIds)
            => _invRepo.RemoveRangeAsync(inventoryIds);
    }
}
