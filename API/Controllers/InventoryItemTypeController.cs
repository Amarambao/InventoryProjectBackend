using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemTypeController : ControllerBase
    {
        private readonly IInventoryItemTypesSrv _invItemTypeSrv;
        private readonly ICheckSrv _checkSrv;

        public InventoryItemTypeController(
            IInventoryItemTypesSrv invItemTypeSrv,
            ICheckSrv checkSrv)
        {
            _invItemTypeSrv = invItemTypeSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("modify")]
        [Authorize]
        public async Task<ActionResult<ResultDto?>> ModifyInventoryItemsRangeAsync([FromBody] IdAndListDto<string> dto)
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

            await _invItemTypeSrv.UpdateInventoryItemTypesAsync(dto.Id, dto.Values);

            return Ok(null);
        }
    }
}
