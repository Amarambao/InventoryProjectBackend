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

        public async Task ModifyInventoryTagsRangeAsync(Guid inventoryId, IEnumerable<string> tagsRequest)
        {
            var inventoryTagsEnt = await _invTagRepo.GetRangeAsync(inventoryId: inventoryId);
            var inventoryTags = inventoryTagsEnt.ToDictionary(i => i.Tag.NormalizedName);

            await _tagSrv.CreateNonExistingAsync(tagsRequest);

            var tags = await _tagSrv.GetTagRangeAsync(tags: tagsRequest);

            var toCreate = new List<InventoryTagEntity>();
            var toRemove = new Dictionary<string, InventoryTagEntity>(inventoryTags);

            foreach (var name in tagsRequest)
            {
                if (inventoryTags.TryGetValue(name.CustomNormalize(), out var existing))
                    toRemove.Remove(existing.Tag.NormalizedName);
                else
                    toCreate.Add(new InventoryTagEntity
                    {
                        InventoryId = inventoryId,
                        TagId = tags.First(i => i.NormalizedName == name.CustomNormalize()).Id,
                    });
            }

            if (toCreate.Any())
                await _invTagRepo.CreateRangeAsync(toCreate);

            if (toRemove.Any())
                await _invTagRepo.RemoveRangeAsync(toRemove.Values);
        }
    }
}
