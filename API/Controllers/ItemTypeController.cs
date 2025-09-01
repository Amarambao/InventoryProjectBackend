using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypeController : ControllerBase
    {
        private readonly IItemTypeSrv _itemTypeSrv;

        public ItemTypeController(IItemTypeSrv itemTypeSrv)
        {
            _itemTypeSrv = itemTypeSrv;
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetAllWPaginationAsync([FromQuery] PaginationRequest dto)
        {
            var result = await _itemTypeSrv.GetNamesWithPaginationAsync(dto);

            return Ok(result);
        }

        [HttpGet("get-by-inventory-id")]
        [AllowAnonymous]
        public async Task<ActionResult<List<IdAndNameDto>>> GetItemRangeAsync([FromQuery] Guid inventoryId)
        {
            var result = (await _itemTypeSrv.GetItemRangeAsync(inventoryId))
                .Select(i => new IdAndNameDto() 
                { 
                    Id = i.Id, 
                    Name = i.Name
                });

            return Ok(result);
        }
    }
}
