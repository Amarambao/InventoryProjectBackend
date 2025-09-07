using CommonLayer.Models.Dto.CustomId;

namespace BusinessLayer.Interfaces
{
    public interface ICustomIdElementSequenceSrv
    {
        Task UpdateCustomIdSequenceAsync(Guid inventoryId, Guid itemId, List<CustomIdElementDto> sequenceDto);
        Task<List<CustomIdElementDto>> GetItemSequenceAsync(Guid inventoryId, Guid itemId);
        Task UpdateIncrementValueAsync(Guid inventoryId, Guid itemId);
        Task<string> GenerateCustomIdAsync(Guid inventoryId, Guid itemId);
    }
}
