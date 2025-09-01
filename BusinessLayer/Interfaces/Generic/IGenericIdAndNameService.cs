using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity.General;

namespace BusinessLayer.Interfaces.Generic
{
    public interface IGenericIdAndNameService<TEntity> where TEntity : IdAndName
    {
        Task<Guid> ManageCreationAsync(string name);
        Task CreateNonExistingAsync(IEnumerable<string> names);
        Task<List<string>> GetNamesWithPaginationAsync(PaginationRequest dto);
    }
}
