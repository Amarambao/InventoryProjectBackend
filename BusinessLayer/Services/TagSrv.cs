using BusinessLayer.Interfaces;
using BusinessLayer.Services.Generic;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class TagSrv : GenericIdAndNameService<TagEntity>, ITagSrv
    {
        private readonly ITagRepo _tagRepo;

        public TagSrv(ITagRepo repo) : base(repo, name => new TagEntity(name))
        {
            _tagRepo = repo;
        }

        public Task<List<TagEntity>> GetTagRangeAsync(Guid? inventoryId = null, IEnumerable<string>? tags = null)
            => _tagRepo.GetTagRangeAsync(inventoryId, tags);
    }
}
