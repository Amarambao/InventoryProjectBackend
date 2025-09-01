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
        public async Task<ActionResult<AppUserGetDto?>> GetMyInfoAsync()
        {
            var result = await _userOpSrv.GetUserByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            return Ok(result);
        }

        [HttpGet("get-by-id")]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserGetDto?>> GetByIdAsync([FromQuery] Guid userId)
        {
            var result = await _userOpSrv.GetUserByIdAsync(userId);

            return Ok(result);
        }

        [HttpPost("change-users-blocking-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto?>> ChangeUsersBlockingStatusAsync([FromBody] ChangeUsersStatusDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            await _userOpSrv.ChangeUsersBlockingStatusAsync(dto);

            return Ok(null);
        }

        [HttpPost("change-users-role-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto?>> ChangeUsersRoleStatusAsync([FromBody] ChangeUsersStatusDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            var result = await _userOpSrv.ChangeUsersRoleStatusAsync(dto);

            return Ok(result);
        }

        [HttpDelete("delete-selected")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultDto?>> DeleteUsersAsync([FromQuery] IEnumerable<Guid> userIds)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var err = new ResultDto(false, "You are blocked");
                return BadRequest(err);
            }

            await _userOpSrv.DeleteUsersAsync(userIds);

            return Ok(null);
        }
    }
}
