using CommonLayer.Models.Dto.CustomDescription;

namespace BusinessLayer.Interfaces
{
    public interface ICustomDescriptionSequenceSrv
    {
        Task ModifyCustomDescriptionSequenceAsync(Guid inventoryId, Guid itemId, List<DescriptionElementDto> sequenceDto);
        Task<IEnumerable<DescriptionElementDto>> GetDescriptionSequenceAsync(Guid inventoryId, Guid itemId);
    }
}
