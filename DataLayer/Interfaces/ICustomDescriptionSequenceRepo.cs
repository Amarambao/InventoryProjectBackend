using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface ICustomDescriptionSequenceRepo
    {
        Task UpdateSequenceAsync(Guid inventoryId, Guid itemId, IEnumerable<CustomDescriptionSequenceEntity> newSequence);
        Task<List<CustomDescriptionSequenceEntity>> GetSequenceAsync(Guid inventoryId, Guid itemId);
    }
}
