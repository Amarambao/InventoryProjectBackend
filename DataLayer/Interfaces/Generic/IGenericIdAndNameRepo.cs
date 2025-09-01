using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity.General;

namespace DataLayer.Interfaces.Generic
{
    public interface IGenericIdAndNameRepo<TEntity> where TEntity : IdAndName
    {
        Task CreateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity?> FindByNameAsync(string search);
        Task<List<string>> GetNamesWithPaginationAsync(PaginationRequest dto);
        Task<List<string>> GetNonExistingNamesAsync(IEnumerable<string> names);
    }
}
