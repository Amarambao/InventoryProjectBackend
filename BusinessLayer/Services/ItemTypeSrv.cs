using BusinessLayer.Interfaces;
using BusinessLayer.Services.Generic;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class ItemTypeSrv : GenericIdAndNameService<ItemTypeEntity>, IItemTypeSrv
    {
        private readonly IItemTypeRepo _itemTypeRepo;

        public ItemTypeSrv(IItemTypeRepo repo) : base(repo, name => new ItemTypeEntity(name))
        {
            _itemTypeRepo = repo;
        }

        public Task<List<ItemTypeEntity>> GetItemRangeAsync(Guid? inventoryId = null, IEnumerable<string>? itemTypes = null)
            => _itemTypeRepo.GetItemRangeAsync(inventoryId, itemTypes);
    }
}
