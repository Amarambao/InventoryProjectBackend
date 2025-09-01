using CommonLayer.Models.Dto.General;
using CommonLayer.Models.Entity.General;
using DataLayer.Interfaces.Generic;

namespace BusinessLayer.Services.Generic
{
    public class GenericIdAndNameService<TEntity> where TEntity : IdAndName
    {
        private readonly IGenericIdAndNameRepo<TEntity> _repo;
        private readonly Func<string, TEntity> _entityFactory;

        public GenericIdAndNameService(IGenericIdAndNameRepo<TEntity> repo, Func<string, TEntity> entityFactory)
        {
            _repo = repo;
            _entityFactory = entityFactory;
        }

        public async Task<Guid> ManageCreationAsync(string name)
        {
            var entity = await _repo.FindByNameAsync(name);
            if (entity is null)
            {
                entity = _entityFactory(name);
                await _repo.CreateRangeAsync(new[] { entity });
            }
            return entity.Id;
        }

        public async Task CreateNonExistingAsync(IEnumerable<string> names)
        {
            var nonExisting = await _repo.GetNonExistingNamesAsync(names);
            if (nonExisting.Any())
            {
                var entities = nonExisting.Select(_entityFactory);
                await _repo.CreateRangeAsync(entities);
            }
        }

        public Task<List<string>> GetNamesWithPaginationAsync(PaginationRequest dto)
            => _repo.GetNamesWithPaginationAsync(dto);
    }
}
