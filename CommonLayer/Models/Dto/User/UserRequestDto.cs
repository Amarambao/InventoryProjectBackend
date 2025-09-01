using CommonLayer.Models.Dto.General;

namespace CommonLayer.Models.Dto.User
{
    public class UserRequestDto : PaginationRequest
    {
        public bool IsIncluded { get; set; }
    }
}
