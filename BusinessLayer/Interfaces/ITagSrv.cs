using BusinessLayer.Interfaces.Generic;
using CommonLayer.Models.Entity;

namespace BusinessLayer.Interfaces
{
    public interface ITagSrv : IGenericIdAndNameService<TagEntity>
    {
        Task<List<TagEntity>> GetTagRangeAsync(Guid? inventoryId = null, IEnumerable<string>? tags = null);
    }
}
