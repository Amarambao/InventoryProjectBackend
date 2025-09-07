using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface ICustomIdElementSequenceRepo
    {
        Task UpdateSequenceAsync(Guid inventoryId, Guid itemId, IEnumerable<CustomIdElementSequenceEntity> newSequence);
        Task<List<CustomIdElementSequenceEntity>> GetItemSequenceAsync(Guid inventoryId, Guid itemId);
        Task<CustomIdElementSequenceEntity?> GetMaxUIntElementStoredAsync(Guid inventoryId, Guid itemId);
        Task UpdateRangeAsync(IEnumerable<CustomIdElementSequenceEntity> storedItems);
    }
}
