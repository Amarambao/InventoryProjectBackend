using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredItemsController : ControllerBase
    {
        private readonly IStoredItemsSrv _storedItemsSrv;
        private readonly ICheckSrv _checkSrv;

        public StoredItemsController(
            IStoredItemsSrv storedItemsSrv,
            ICheckSrv checkSrv)
        {
            _storedItemsSrv = storedItemsSrv;
            _checkSrv = checkSrv;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<ResultDto<AddItemDto?>>> AddItemAsync([FromBody] AddItemDto dto)
        {
            if (await _checkSrv.CheckUserStatus(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)))
            {
                var resultDto = new ResultDto<Guid>(false, "You are blocked");
                return BadRequest(resultDto);
            }

            if (!(await _checkSrv.IsUserInventoryEditorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), dto.InventoryId)
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            var result = await _storedItemsSrv.AddItemAsync(dto, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            return Ok(result);
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StoredItemGetAllDto>>> GetAllWPagination([FromQuery] PaginationRequest dto)
        {
            if (!dto.InventoryId.HasValue)
                return Ok(new());

            var result = await _storedItemsSrv.GetAllWPaginationAsync(dto);

            return Ok(result);
        }

        [HttpGet("get-full")]
        [AllowAnonymous]
        public async Task<ActionResult<StoredItemGetFullDto>> GetFullInfoAsync([FromQuery] Guid itemId)
        {
            var result = await _storedItemsSrv.GetFullInfoAsync(itemId);

            return Ok(result);
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

            if (!(await _checkSrv.IsUserInventoryEditorAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), dto.Id)
                || User.IsInRole("admin")))
            {
                var checkResult = new ResultDto(false, "You are not allowed to edit inventory");
                return Ok(checkResult);
            }

            await _storedItemsSrv.RemoveRangeAsync(dto.Values);

            return Ok(null);
        }
    }
}
