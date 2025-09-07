using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventorySrv _invSrv;
        private readonly ICheckSrv _checkSrv;

        public InventoryController(
            IInventorySrv invSrv,
            ICheckSrv checkSrv)
        {
            _invSrv = invSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<ResultDto<Guid>>> CreateInventoryAsync([FromBody] InventoryCreateDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto<Guid>(false, "You are blocked"));

            var inventoryId = await _invSrv.CreateInventoryAsync(dto, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            return Ok(new ResultDto<Guid>(true, null, inventoryId));
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<InventoryGetLiteDto>>> GetInventoriesLiteAsync([FromQuery] InventoryRequestDto dto)
        {
            var result = await _invSrv.GetInventoriesLiteAsync(dto);

            return Ok(result);
        }

        [HttpGet("get-full")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultDto<InventoryGetFullDto>>> GetInventoryFullAsync([FromQuery] Guid inventoryId)
        {
            var result = await _invSrv.GetInventoryFullAsync(inventoryId);

            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult<ResultDto>> UpdateInventoryAsync([FromBody] InventoryUpdateDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), [dto.InventoryId])
                || User.IsInRole("admin")))
                return Ok(new ResultDto(false, "You are not allowed to edit inventory"));

            var result = await _invSrv.UpdateInventoryAsync(dto);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult<ResultDto>> RemoveInventoryRangeAsync([FromQuery] IEnumerable<Guid> inventoryIds)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
                return BadRequest(new ResultDto(false, "You are blocked"));

            var checkResult = new ResultDto(true);

            if (!(await _checkSrv.IsInventoryCreatorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), inventoryIds)
                || User.IsInRole("admin")))
                checkResult = new ResultDto(false, "You are not allowed to remove some of inventories");

            await _invSrv.RemoveInventoryRangeAsync(inventoryIds);

            return Ok(checkResult);
        }
    }
}
