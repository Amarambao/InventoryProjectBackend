using CommonLayer.Models.Dto.CustomId;

namespace BusinessLayer.Interfaces
{
    public interface ICustomIdElementSequenceSrv
    {
        Task ModifyCustomIdSequenceAsync(Guid inventoryId, Guid itemId, List<CustomIdElementDto> sequenceDto);
        Task<List<CustomIdElementDto>> GetItemSequenceAsync(Guid inventoryId, Guid itemId);
        Task<string> GenerateCustomIdAsync(Guid inventoryId, Guid itemId);
    }
}
