using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;

namespace BusinessLayer.Interfaces
{
    public interface IUserOperationsSrv
    {
        Task<IEnumerable<AppUserGetDto>> GetAllWPaginationAsync(UserRequestDto dto);
        Task<AppUserGetDto?> GetUserByIdAsync(Guid userId);
        Task ChangeUsersBlockingStatusAsync(ChangeUsersStatusDto dto);
        Task<ResultDto?> ChangeUsersRoleStatusAsync(ChangeUsersStatusDto dto);
        Task DeleteUsersAsync(IEnumerable<Guid> userIds);
    }
}
