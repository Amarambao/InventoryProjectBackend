using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using CommonLayer.Models.Entity;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace BusinessLayer.Services
{
    public class AppUserSrv : IAppUserSrv
    {
        private readonly IJwtSrv _jwtService;
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly SignInManager<AppUserEntity> _signInManager;

        public AppUserSrv(IJwtSrv jwtService, UserManager<AppUserEntity> userManager, SignInManager<AppUserEntity> signInManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResultDto<string>> LoginAsync(LoginDto dto)
        {
            AppUserEntity? user = null;

            if (dto.EmailOrUserName.Contains('@'))
                user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);
            else
                user = await _userManager.FindByNameAsync(dto.EmailOrUserName);

            if (user is null)
                return new(false, "User not found");

            if (user.IsBlocked)
                return new(false, "This user is blocked");

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!passwordCheck.Succeeded)
                return new(false, "Wrong credentials");

            var token = await _jwtService.GenerateJwtTokenAsync(user);

            return new(true, null, token);
        }

        public async Task<ResultDto<string>> RegisterUserAsync(RegistrationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return new(false, "Required fields are empty");

            var user = new AppUserEntity(dto, _userManager.NormalizeName(dto.UserName), _userManager.NormalizeEmail(dto.Email));

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();

                result.Errors.Select(err => sb.Append($"{err.Description}\n"));
                
                return new(false, sb.ToString());
            }

            return new(true, null, await _jwtService.GenerateJwtTokenAsync(user));
        }
    }
}
