using CommonLayer.Models.Entity;
using DataLayer.Interfaces.Generic;

namespace DataLayer.Interfaces
{
    public interface ITagRepo : IGenericIdAndNameRepo<TagEntity>
    {
        Task<List<TagEntity>> GetTagRangeAsync(Guid? inventoryId = null, IEnumerable<string>? itemTypes = null);
    }
}
