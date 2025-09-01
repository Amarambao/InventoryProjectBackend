using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTypeController : ControllerBase
    {
        private readonly IInventoryTypeSrv _invTypeSrv;

        public InventoryTypeController(IInventoryTypeSrv invTypeSrv)
        {
            _invTypeSrv = invTypeSrv;
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUserGetDto>>> GetAllWPaginationAsync([FromQuery] PaginationRequest dto)
        {
            var result = await _invTypeSrv.GetNamesWithPaginationAsync(dto);

            return Ok(result);
        }
    }
}
