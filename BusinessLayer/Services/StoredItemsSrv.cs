using BusinessLayer.Interfaces;
using CommonLayer.Enum;
using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Item;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessLayer.Services
{
    public class StoredItemsSrv : IStoredItemsSrv
    {
        private readonly IStoredItemsRepo _storedItemsRepo;
        private readonly ICustomIdElementSequenceSrv _customIdElementSequenceSrv;
        private readonly ICustomDescriptionSequenceSrv _customDescriptionSequenceSrv;
        private readonly IItemTypeSrv _itemTypeSrv;
        private readonly UserManager<AppUserEntity> _userManager;

        public StoredItemsSrv(
            IStoredItemsRepo storedItemsRepo,
            ICustomIdElementSequenceSrv customIdElementSequenceSrv,
            ICustomDescriptionSequenceSrv customDescriptionSequenceSrv,
            IItemTypeSrv itemTypeSrv,
            UserManager<AppUserEntity> userManager)
        {
            _storedItemsRepo = storedItemsRepo;
            _customIdElementSequenceSrv = customIdElementSequenceSrv;
            _customDescriptionSequenceSrv = customDescriptionSequenceSrv;
            _itemTypeSrv = itemTypeSrv;
            _userManager = userManager;
        }

        public async Task<ResultDto<AddItemDto?>> AddItemAsync(AddItemDto dto, Guid creatorId)
        {
            var requestedItem = (await _itemTypeSrv.GetItemRangeAsync(dto.InventoryId)).FirstOrDefault(i => i.Id == dto.ItemTypeId);
            if (requestedItem is null)
                return new(false, "Requested item not found");

            var creator = await _userManager.FindByIdAsync(creatorId.ToString());
            if (creator is null)
                return new(false, "Creator not found");

            var customId = dto.CustomId ?? await _customIdElementSequenceSrv.GenerateCustomIdAsync(dto.InventoryId, requestedItem.Id);

            var item = new StoredItemsEntity(dto, creator, requestedItem.Id, customId);

            var descriptionResult = await AddStoredItemDescriptionByDtoAsync(item, dto);

            if (!descriptionResult.IsSucceeded)
                return new(false, descriptionResult.Error, new(item, requestedItem.Id));

            if (await _storedItemsRepo.IsItemExistAsync(dto.InventoryId, requestedItem.Id, item.CustomId))
                return new(false, "Custom id already exist", new(item, requestedItem.Id));

            await _storedItemsRepo.AddItemAsync(item);

            await _customIdElementSequenceSrv.UpdateIncrementValueAsync(dto.InventoryId, requestedItem.Id);

            return new(true);
        }

        public async Task<IEnumerable<StoredItemGetAllDto>> GetAllWPaginationAsync(PaginationRequest dto)
            => (await _storedItemsRepo.GetStoredItemsWPaginationAsync(dto))
            .GroupBy(g => g.InventoryItemType.Item)
            .Select(i => new StoredItemGetAllDto
            {
                ItemId = i.Key.Id,
                ItemName = i.Key.Name,
                StoredItemsId = i.Select(x => new StoredItemGetLiteDto(x))
            });

        public async Task<StoredItemGetFullDto?> GetFullInfoAsync(Guid itemId)
        {
            var itemEntity = await _storedItemsRepo.GetFullInfoAsync(itemId);

            return itemEntity is not null ? new(itemEntity) : null;
        }

        public Task RemoveRangeAsync(IEnumerable<Guid> storedItemIds)
            => _storedItemsRepo.RemoveRangeAsync(storedItemIds);

        private async Task<ResultDto<StoredItemsEntity>> AddStoredItemDescriptionByDtoAsync(StoredItemsEntity item, AddItemDto dto)
        {
            var descriptionSequence = (await _customDescriptionSequenceSrv.GetDescriptionSequenceAsync(dto.InventoryId, item.ItemId)).ToList();

            if (descriptionSequence.Count != dto.ItemDescription.Count)
                return new(false, "Mismatch in descriptionTypes");

            for (int i = 0; i < descriptionSequence.Count; i++)
            {
                if (descriptionSequence[i].DescriptionType != dto.ItemDescription[i].DescriptionType)
                    return new(false, "Mismatch in descriptionTypes");

                item.StoredItemDescriptions.Add(new StoredItemDescriptionEntity(item, dto.ItemDescription[i], i, descriptionSequence[i].Name));
            }
            return new(true, null, item);
        }
    }
}
