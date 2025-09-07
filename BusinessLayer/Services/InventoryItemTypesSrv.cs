using BusinessLayer.Interfaces;
using CommonLayer.Extensions;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class InventoryItemTypesSrv : IInventoryItemTypesSrv
    {
        private readonly IInventoryItemTypesRepo _invItemTypeRepo;
        private readonly IItemTypeSrv _itemTypeSrv;

        public InventoryItemTypesSrv(
            IInventoryItemTypesRepo invItemTypeRepo,
            IItemTypeSrv itemTypeSrv)
        {
            _invItemTypeRepo = invItemTypeRepo;
            _itemTypeSrv = itemTypeSrv;
        }

        public async Task UpdateInventoryItemTypesAsync(Guid inventoryId, IEnumerable<string> itemsRequest)
        {
            await _itemTypeSrv.CreateNonExistingAsync(itemsRequest);

            var itemTypes = (await _itemTypeSrv.GetItemRangeAsync(itemTypes: itemsRequest)).ToDictionary(i => i.NormalizedName, i => i.Id);

            var inventoryItemTypesToUpgrade = itemsRequest
                .Select(r => r.CustomNormalize())
                .Where(n => itemTypes.ContainsKey(n))
                .Select(n => new InventoryItemTypesEntity
                {
                    InventoryId = inventoryId,
                    ItemId = itemTypes[n]
                })
                .ToList();

            await _invItemTypeRepo.UpdateInventoryItemTypesAsync(inventoryId, inventoryItemTypesToUpgrade);
        }
    }
}
