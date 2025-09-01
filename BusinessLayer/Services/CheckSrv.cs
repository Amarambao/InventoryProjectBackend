using BusinessLayer.Interfaces;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services
{
    public class CheckSrv : ICheckSrv
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly ICheckRepo _checkRepo;

        public CheckSrv(
            UserManager<AppUserEntity> userManager,
            ICheckRepo checkRepo)
        {
            _userManager = userManager;
            _checkRepo = checkRepo;
        }

        public async Task<bool> CheckUserStatus(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user is not null ? user.IsBlocked : true;
        }

        public async Task<bool> IsInventoryCreatorAsync(Guid userId, IEnumerable<Guid> inventoryIds)
            => (await _checkRepo.GetWhereInventoryCreatorAsync(userId, inventoryIds)).Count() == inventoryIds.Count();

        public Task<bool> IsUserInventoryEditorAsync(Guid userId, Guid inventoryId)
            => _checkRepo.GetWhereInventoryEditorAsync(userId, inventoryId);
    }
}
