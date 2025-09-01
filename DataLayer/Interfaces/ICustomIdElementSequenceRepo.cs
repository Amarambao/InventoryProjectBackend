using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface ICustomIdElementSequenceRepo
    {
        Task CreateSequenceAsync(IEnumerable<CustomIdElementSequenceEntity> sequence);
        Task<List<CustomIdElementSequenceEntity>> GetItemSequenceAsync(Guid inventoryId, Guid itemId);
        Task UpdateRangeAsync(IEnumerable<CustomIdElementSequenceEntity> storedItems);
        Task RemoveRangeAsync(IEnumerable<CustomIdElementSequenceEntity> storedItems);
    }
}
