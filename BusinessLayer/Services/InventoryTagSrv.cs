using BusinessLayer.Interfaces;
using CommonLayer.Extensions;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class InventoryTagSrv : IInventoryTagSrv
    {
        private readonly IInventoryTagRepo _invTagRepo;
        private readonly ITagSrv _tagSrv;

        public InventoryTagSrv(
            IInventoryTagRepo invTagRepo,
            ITagSrv tagSrv)
        {
            _invTagRepo = invTagRepo;
            _tagSrv = tagSrv;
        }

        public async Task UpdateInventoryTagsAsync(Guid inventoryId, IEnumerable<string> tagsRequest)
        {
            await _tagSrv.CreateNonExistingAsync(tagsRequest);

            var tags = (await _tagSrv.GetTagRangeAsync(tags: tagsRequest)).ToDictionary(i => i.NormalizedName, i => i.Id);
            
            var inventoryTagsToUpgrade = tagsRequest
                .Select(r => r.CustomNormalize())
                .Where(n => tags.ContainsKey(n))
                .Select(n => new InventoryTagEntity
                {
                    InventoryId = inventoryId,
                    TagId = tags[n]
                })
                .ToList();

            await _invTagRepo.UpdateInventoryTagsAsync(inventoryId, inventoryTagsToUpgrade);
        }
    }
}
