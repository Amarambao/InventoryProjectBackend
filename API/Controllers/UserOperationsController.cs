using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationsController : ControllerBase
    {
        private readonly IUserOperationsSrv _userOpSrv;
        private readonly ICheckSrv _checkSrv;

        public UserOperationsController(
            IUserOperationsSrv userOpSrv,
            ICheckSrv checkSrv)
        {
            _userOpSrv = userOpSrv;
            _checkSrv = checkSrv;
        }

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUserGetDto>>> GetAllWPaginationAsync([FromQuery] UserRequestDto dto)
        {
            var result = await _userOpSrv.GetAllWPaginationAsync(dto);

            return Ok(result);
        }

        [HttpGet("get-my-info")]
        [Authorize]
        public async Task<ActionResult<ResultDto<AppUserGetDto>>> GetMyInfoAsync()
        {
            var result = await _userOpSrv.GetUserByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            return Ok(result);
        }

        [HttpGet("get-by-id")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultDto<AppUserGetDto>>> GetByIdAsync([FromQuery] Guid userId)
        {
            var result = await _userOpSrv.GetUserByIdAsync(userId);

            return Ok(result);
        }

        [HttpPost("update-main-info")]
        [Authorize]
        public async Task<ActionResult<ResultDto>> UpdateUserMainInfoAsync([FromBody] UpdateUserMainInfoDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            if (Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!) != dto.Id || !User.IsInRole("admin"))
                return Ok(new ResultDto(false, "You are not allowed to edit this user"));

            var result = await _userOpSrv.UpdateUserMainInfoAsync(dto);

            return Ok(result);
        }

        [HttpPost("change-users-blocking-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto>> ChangeUsersBlockingStatusAsync([FromBody] ChangeUsersStatusDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            var result = await _userOpSrv.ChangeUsersBlockingStatusAsync(dto);

            return Ok(result);
        }

        [HttpPost("change-users-role-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto>> ChangeUsersRoleStatusAsync([FromBody] ChangeUsersStatusDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            var result = await _userOpSrv.ChangeUsersRoleStatusAsync(dto);

            return Ok(result);
        }

        [HttpDelete("delete-selected")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto>> DeleteUsersAsync([FromQuery] IEnumerable<Guid> userIds)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            var result = await _userOpSrv.DeleteUsersAsync(userIds);

            return Ok(result);
        }
    }
}
