using BusinessLayer.Interfaces;
using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using CommonLayer.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class UserOperationsSrv : IUserOperationsSrv
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserOperationsSrv(
            UserManager<AppUserEntity> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<AppUserGetDto>> GetAllWPaginationAsync(UserRequestDto dto)
        {
            var query = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(dto.SearchValue))
            {
                query = query.Where(u =>
                    u.NormalizedUserName!.Contains(dto.SearchValue.CustomNormalize()) ||
                    u.NormalizedEmail!.Contains(dto.SearchValue.CustomNormalize()) ||
                    u.NormalizedName.Contains(dto.SearchValue.CustomNormalize()));
            }

            if (dto.InventoryId.HasValue)
            {
                if (dto.IsIncluded)
                    query = query.Where(u => u.UserInventories.Any(ui => ui.InventoryId == dto.InventoryId.Value));
                else
                    query = query.Where(u => !u.UserInventories.Any(ui => ui.InventoryId == dto.InventoryId.Value));
            }

            query = query
                .OrderBy(u => u.NormalizedName)
                .Skip(dto.Page * dto.ReturnCount)
                .Take(dto.ReturnCount);

            var users = await query.ToListAsync();

            var result = new List<AppUserGetDto>(users.Count);
            foreach (var user in users)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
                result.Add(new AppUserGetDto(user, isAdmin));
            }

            return result;
        }

        public async Task<AppUserGetDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return null;

            return new AppUserGetDto(user, await _userManager.IsInRoleAsync(user, "admin"));
        }

        public async Task ChangeUsersBlockingStatusAsync(ChangeUsersStatusDto dto)
        {
            var users = await _userManager.Users.Where(z => dto.UserIds.Contains(z.Id)).ToListAsync();

            foreach (var user in users)
            {
                if (user.IsBlocked == dto.RequestedStatus)
                    continue;

                user.IsBlocked = dto.RequestedStatus;

                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<ResultDto?> ChangeUsersRoleStatusAsync(ChangeUsersStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
                return new(false, "Role name is empty");

            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role is null)
                return new(false, "Role not found");

            var users = await _userManager.Users.Where(u => dto.UserIds.Contains(u.Id)).ToListAsync();
            if (!users.Any())
                return new(false, "No users found");

            if (dto.RequestedStatus)
                await AddUsersToRoleAsync(users, dto.RoleName);
            else
                await RemoveUsersFromRoleAsync(users, dto.RoleName);

            return null;
        }

        public async Task DeleteUsersAsync(IEnumerable<Guid> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            foreach (var user in users)
                await _userManager.DeleteAsync(user);
        }

        private async Task AddUsersToRoleAsync(IEnumerable<AppUserEntity> users, string roleName)
        {
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "admin"))
                    continue;

                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        private async Task RemoveUsersFromRoleAsync(IEnumerable<AppUserEntity> users, string roleName)
        {
            foreach (var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, "admin"))
                    continue;

                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }
    }
}
