using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;

namespace BusinessLayer.Interfaces
{
    public interface IAppUserSrv
    {
        public Task<ResultDto<string>> LoginAsync(LoginDto dto);
        public Task<ResultDto<string>> RegisterUserAsync(RegistrationDto dto);
    }
}
