using BusinessLayer.Interfaces;
using CommonLayer.Extensions;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserOperationsSrv : IUserOperationsSrv
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IInventorySrv _invSrv;

        public UserOperationsSrv(
            UserManager<AppUserEntity> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IInventorySrv invSrv,
            PostgreSQLContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _invSrv = invSrv;
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

        public async Task<ResultDto<AppUserGetDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return new(false, "User not found");

            return new(true, null, new AppUserGetDto(user, await _userManager.IsInRoleAsync(user, "admin")));
        }

        public async Task<ResultDto> UpdateUserMainInfoAsync(UpdateUserMainInfoDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());

            if (user == null)
                return new(false, "User not found");

            if (user.ConcurrencyStamp != dto.ConcurrencyStamp)
                return new(false, "User already changed. Update the page");

            if (!string.IsNullOrWhiteSpace(dto.FullName) && dto.FullName.Trim() != user.Name)
            {
                user.Name = dto.FullName;
                user.NormalizedName = dto.FullName.CustomNormalize();
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName.Trim() != user.UserName)
            {
                user.UserName = dto.UserName;
                user.NormalizedUserName = _userManager.NormalizeName(dto.UserName);
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email.Trim() != user.Email)
            {
                user.Email = dto.Email;
                user.Email = _userManager.NormalizeEmail(dto.Email);
            }

            var identityResult = await _userManager.UpdateAsync(user);

            var sb = new StringBuilder();

            if (!identityResult.Succeeded)
                identityResult.Errors.Select(err => sb.Append($"{err.Description}\n"));

            return new(identityResult.Succeeded, sb.ToString());
        }

        public async Task<ResultDto> ChangeUsersBlockingStatusAsync(ChangeUsersStatusDto dto)
        {
            var users = await _userManager.Users.Where(u => dto.UserIdAndStamp.Select(dto => dto.Id).Contains(u.Id)).ToListAsync();

            var resultDto = new ResultDto() { IsSucceeded = true };
            var sb = new StringBuilder();

            foreach (var user in users)
            {
                if (user.ConcurrencyStamp != dto.UserIdAndStamp.FirstOrDefault(u => u.Id == user.Id)!.Value)
                {
                    resultDto.IsSucceeded = false;
                    sb.Append("Some of the users have been modified\nPlease reload page.\n");
                }

                if (user.IsBlocked == dto.RequestedStatus)
                    continue;

                user.IsBlocked = dto.RequestedStatus;

                var identityResult = await _userManager.UpdateAsync(user);

                if (!identityResult.Succeeded)
                    identityResult.Errors.Select(err => sb.Append($"{err.Description}\n"));
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                resultDto.IsSucceeded = false;
                resultDto.Error = sb.ToString();
            }

            return resultDto;
        }

        public async Task<ResultDto> ChangeUsersRoleStatusAsync(ChangeUsersStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
                return new(false, "Role name is empty");

            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role is null)
                return new(false, "Role not found");

            var users = await _userManager.Users.Where(u => dto.UserIdAndStamp.Select(dto => dto.Id).Contains(u.Id)).ToListAsync();
            if (!users.Any())
                return new(false, "No users found");

            var validUsers = users
                .Where(u => dto.UserIdAndStamp.FirstOrDefault(x => x.Id == u.Id)?.Value == u.ConcurrencyStamp)
                .ToList();

            var result = new ResultDto() { IsSucceeded = true };

            if (validUsers.Count != users.Count)
                result = new(false, "Some users were changed\n");

            Func<List<AppUserEntity>, string, Task<ResultDto>> roleOperation = dto.RequestedStatus ? AddUsersToRoleAsync : RemoveUsersFromRoleAsync;

            var operationResult = await roleOperation(validUsers, dto.RoleName);

            if (!operationResult.IsSucceeded)
                result.Error += operationResult.Error;

            return result;
        }

        public async Task<ResultDto> DeleteUsersAsync(IEnumerable<Guid> userIds)
        {
            var result = new ResultDto() { IsSucceeded = true };
            var sb = new StringBuilder();

            var users = await _userManager.Users
                .Include(u => u.UserInventories)
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                if (user.UserInventories is not null && user.UserInventories.Any())
                {
                    var creatorInventoryIds = user.UserInventories
                        .Where(ui => ui.IsCreator)
                        .Select(ui => ui.InventoryId)
                        .ToList();

                    if (creatorInventoryIds.Count > 0)
                        await _invSrv.RemoveInventoryRangeAsync(creatorInventoryIds);
                }

                var identityResult = await _userManager.DeleteAsync(user);

                if (!identityResult.Succeeded)
                    identityResult.Errors.Select(err => sb.Append($"{err.Description}\n"));
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                result.IsSucceeded = false;
                result.Error = sb.ToString();
            }

            return result;
        }

        private async Task<ResultDto> AddUsersToRoleAsync(IEnumerable<AppUserEntity> users, string roleName)
        {
            var result = new ResultDto() { IsSucceeded = true };
            var sb = new StringBuilder();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                    continue;

                var identityResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!identityResult.Succeeded)
                    identityResult.Errors.Select(err => sb.Append($"{err.Description}\n"));
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                result.IsSucceeded = false;
                result.Error = sb.ToString();
            }

            return result;
        }

        private async Task<ResultDto> RemoveUsersFromRoleAsync(IEnumerable<AppUserEntity> users, string roleName)
        {
            var result = new ResultDto() { IsSucceeded = true };
            var sb = new StringBuilder();

            foreach (var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, roleName))
                    continue;

                var identityResult = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (!identityResult.Succeeded)
                    identityResult.Errors.Select(err => sb.Append($"{err.Description}\n"));
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                result.IsSucceeded = false;
                result.Error = sb.ToString();
            }

            return result;
        }
    }
}
