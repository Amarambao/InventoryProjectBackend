using BusinessLayer.Interfaces;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class InventoryEditorsSrv : IInventoryEditorsSrv
    {
        private readonly IInventoryEditorsRepo _userInventoryRepo;

        public InventoryEditorsSrv(IInventoryEditorsRepo userInventoryRepo)
        {
            _userInventoryRepo = userInventoryRepo;
        }

        public Task AddRange(Guid inventoryId, IEnumerable<Guid> userIds)
            => _userInventoryRepo.CreateRangeAsync(userIds.Select(x => new InventoryEditorsEntity()
            {
                InventoryId = inventoryId,
                UserId = x,
                IsCreator = false
            }));

        public async Task RemoveRangeAsync(Guid inventoryId, IEnumerable<Guid> userIds)
        {
            var userInventories = await _userInventoryRepo.GetInventoryEditorsAsync(inventoryId, userIds);

            await _userInventoryRepo.RemoveRangeAsync(userInventories.Where(ui => !ui.IsCreator));
        }
    }
}
