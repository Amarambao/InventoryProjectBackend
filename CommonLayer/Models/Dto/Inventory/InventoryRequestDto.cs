using CommonLayer.Models.Dto.General;

namespace CommonLayer.Models.Dto.Inventory
{
    public class InventoryRequestDto : PaginationRequest
    {
        public Guid? UserId { get; set; }
        public bool? IsCreator { get; set; }
    }
}
