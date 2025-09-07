using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;

namespace BusinessLayer.Interfaces
{
    public interface IUserOperationsSrv
    {
        Task<IEnumerable<AppUserGetDto>> GetAllWPaginationAsync(UserRequestDto dto);
        Task<ResultDto<AppUserGetDto>> GetUserByIdAsync(Guid userId);
        Task<ResultDto> UpdateUserMainInfoAsync(UpdateUserMainInfoDto dto);
        Task<ResultDto> ChangeUsersBlockingStatusAsync(ChangeUsersStatusDto dto);
        Task<ResultDto> ChangeUsersRoleStatusAsync(ChangeUsersStatusDto dto);
        Task<ResultDto> DeleteUsersAsync(IEnumerable<Guid> userIds);
    }
}
