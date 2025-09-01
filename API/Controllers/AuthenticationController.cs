using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAppUserSrv _authSrv;

        public AuthenticationController(IAppUserSrv authSrv)
        {
            _authSrv = authSrv;
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultDto<string>>> SignUpAsync([FromBody] RegistrationDto dto)
        {
            var result = await _authSrv.RegisterUserAsync(dto);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultDto<string>>> SignInAsync([FromBody] LoginDto dto)
        {
            var result = await _authSrv.LoginAsync(dto);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }
    }
}
