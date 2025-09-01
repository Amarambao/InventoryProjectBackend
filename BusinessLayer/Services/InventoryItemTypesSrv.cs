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

        public async Task ModifyInventoryItemsRangeAsync(Guid inventoryId, IEnumerable<string> itemsRequest)
        {
            var inventoryItems = (await _invItemTypeRepo.GetRangeAsync(inventoryId: inventoryId)).ToDictionary(i => i.Item.NormalizedName);

            await _itemTypeSrv.CreateNonExistingAsync(itemsRequest);

            var itemTypes = await _itemTypeSrv.GetItemRangeAsync(itemTypes: itemsRequest);

            var toCreate = new List<InventoryItemTypesEntity>();
            var toRemove = new Dictionary<string, InventoryItemTypesEntity>(inventoryItems);

            foreach (var name in itemsRequest)
            {
                if (inventoryItems.TryGetValue(name.CustomNormalize(), out var existing))
                    toRemove.Remove(existing.Item.NormalizedName);
                else
                    toCreate.Add(new InventoryItemTypesEntity
                    {
                        InventoryId = inventoryId,
                        ItemId = itemTypes.First(i => i.NormalizedName == name.CustomNormalize()).Id,
                    });
            }

            if (toCreate.Any())
                await _invItemTypeRepo.CreateRangeAsync(toCreate);

            if (toRemove.Any())
                await _invItemTypeRepo.RemoveRangeAsync(toRemove.Values);
        }
    }
}
