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
    public class InventoryEditorsController : ControllerBase
    {
        private readonly IInventoryEditorsSrv _invEditorsSrv;
        private readonly ICheckSrv _checkSrv;

        public InventoryEditorsController(
            IInventoryEditorsSrv inventoryEditorsSrv,
            ICheckSrv checkSrv)
        {
            _invEditorsSrv = inventoryEditorsSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> AddRange([FromBody] IdAndListDto<Guid> dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto<Guid>(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [dto.Id])
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _invEditorsSrv.AddRange(dto.Id, dto.Values);

            return Ok(null);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> RemoveRangeAsync([FromQuery] IdAndListDto<Guid> dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto<Guid>(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [dto.Id])
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _invEditorsSrv.RemoveRangeAsync(dto.Id, dto.Values);

            return Ok();
        }
    }
}
