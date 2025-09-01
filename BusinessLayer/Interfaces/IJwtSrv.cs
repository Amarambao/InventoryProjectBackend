using CommonLayer.Models.Entity;

namespace BusinessLayer.Interfaces
{
    public interface IJwtSrv
    {
        public Task<string> GenerateJwtTokenAsync(AppUserEntity user);
    }
}
