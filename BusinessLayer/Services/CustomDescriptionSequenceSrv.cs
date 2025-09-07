using BusinessLayer.Interfaces;
using CommonLayer.Enum;
using CommonLayer.Models.Dto.CustomDescription;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CustomDescriptionSequenceSrv : ICustomDescriptionSequenceSrv
    {
        private readonly ICustomDescriptionSequenceRepo _customDescriptionSequenceRepo;

        public CustomDescriptionSequenceSrv(ICustomDescriptionSequenceRepo customDescriptionSequenceRepo)
        {
            _customDescriptionSequenceRepo = customDescriptionSequenceRepo;
        }

        public async Task ModifyCustomDescriptionSequenceAsync(Guid inventoryId, Guid itemId, List<DescriptionElementDto> sequenceDto)
        {
            var sequenceToUpdate = new List<CustomDescriptionSequenceEntity>();

            for (int i = 0; i < sequenceDto.Count; i++)
                sequenceToUpdate.Add(new CustomDescriptionSequenceEntity()
                {
                    Id = Guid.NewGuid(),
                    InventoryId = inventoryId,
                    ItemId = itemId,
                    DescripionType = sequenceDto[i].DescriptionType,
                    Name = sequenceDto[i].Name,
                    Order = i
                });

            await _customDescriptionSequenceRepo.UpdateSequenceAsync(inventoryId, itemId, sequenceToUpdate);
        }

        public async Task<IEnumerable<DescriptionElementDto>> GetDescriptionSequenceAsync(Guid inventoryId, Guid itemId)
            => (await _customDescriptionSequenceRepo.GetSequenceAsync(inventoryId, itemId)).Select(s => new DescriptionElementDto()
            {
                Name = s.Name,
                DescriptionType = s.DescripionType,
            });
    }
}
