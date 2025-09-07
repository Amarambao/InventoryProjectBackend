using BusinessLayer.Interfaces;
using CommonLayer.Enum;
using CommonLayer.Models.Dto.CustomId;
using CommonLayer.Models.Entity;
using DataLayer.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services
{
    public class CustomIdElementSequenceSrv : ICustomIdElementSequenceSrv
    {
        private readonly ICustomIdElementSequenceRepo _customIdElementSequenceRepo;

        public CustomIdElementSequenceSrv(ICustomIdElementSequenceRepo customIdElementSequenceRepo)
        {
            _customIdElementSequenceRepo = customIdElementSequenceRepo;
        }

        public async Task UpdateCustomIdSequenceAsync(Guid inventoryId, Guid itemId, List<CustomIdElementDto> sequenceDto)
        {
            var sequenceEntitiesUpdate = new List<CustomIdElementSequenceEntity>();

            var incrementValue = 0;

            if (sequenceDto.Any(s => s.ElementType == CustomIdElementEnum.UIntSequence))
            {
                var incrementElement = await _customIdElementSequenceRepo.GetMaxUIntElementStoredAsync(inventoryId, itemId);
                if (incrementElement is not null)
                    incrementValue = incrementElement.IncrementValue!.Value;
            }

            for (int i = 0; i < sequenceDto.Count; i++)
            {
                sequenceEntitiesUpdate.Add(new CustomIdElementSequenceEntity()
                {
                    Id = Guid.NewGuid(),
                    InventoryId = inventoryId,
                    ItemId = itemId,
                    ElementType = sequenceDto[i].ElementType,
                    Order = i,
                    FixedTextValue = sequenceDto[i].ElementType == CustomIdElementEnum.FixedText ? sequenceDto[i].FixedTextValue : null,
                    IncrementValue = sequenceDto[i].ElementType == CustomIdElementEnum.UIntSequence ? incrementValue : null,
                });
            }

            await _customIdElementSequenceRepo.UpdateSequenceAsync(inventoryId, itemId, sequenceEntitiesUpdate);
        }

        public async Task<List<CustomIdElementDto>> GetItemSequenceAsync(Guid inventoryId, Guid itemId)
        {
            var result = await _customIdElementSequenceRepo.GetItemSequenceAsync(inventoryId, itemId);

            if (!result.Any())
                return new List<CustomIdElementDto>();

            return result.Select(s => new CustomIdElementDto
            {
                ElementType = s.ElementType,
                FixedTextValue = s.FixedTextValue,
                IncrementValue = s.IncrementValue,
            }).ToList();
        }

        public async Task UpdateIncrementValueAsync(Guid inventoryId, Guid itemId)
        {
            var uintElement = await _customIdElementSequenceRepo.GetMaxUIntElementStoredAsync(inventoryId, itemId);

            if (uintElement is null)
                return;

            uintElement.IncrementValue++;

            await _customIdElementSequenceRepo.UpdateRangeAsync([uintElement]);
        }

        public async Task<string> GenerateCustomIdAsync(Guid inventoryId, Guid itemId)
        {
            var elements = await _customIdElementSequenceRepo.GetItemSequenceAsync(inventoryId, itemId);
            var sb = new StringBuilder();

            var incrementValue = 0;
            if (elements.Any(e => e.ElementType == CustomIdElementEnum.UIntSequence))
            {
                var uintElement = await _customIdElementSequenceRepo.GetMaxUIntElementStoredAsync(inventoryId, itemId);

                if (uintElement is not null)
                    incrementValue = uintElement.IncrementValue!.Value;
            }

            foreach (var element in elements)
            {
                switch (element.ElementType)
                {
                    case CustomIdElementEnum.FixedText:
                        sb.Append($"{element.FixedTextValue ?? string.Empty}-");
                        break;
                    case CustomIdElementEnum.Random20Bit:
                        sb.Append($"{RandomNumberGenerator.GetInt32(1 << 20)}-");
                        break;
                    case CustomIdElementEnum.Random32Bit:
                        sb.Append($"{BitConverter.ToUInt32(RandomNumberGenerator.GetBytes(4))}-");
                        break;
                    case CustomIdElementEnum.Random6Digit:
                        sb.Append($"{RandomNumberWithMinValue(99999, 1000000)}-");
                        break;
                    case CustomIdElementEnum.Random9Digit:
                        sb.Append($"{RandomNumberWithMinValue(99999999, 1000000000)}-");
                        break;
                    case CustomIdElementEnum.Guid:
                        sb.Append($"{Guid.NewGuid()}-");
                        break;
                    case CustomIdElementEnum.DateTime:
                        sb.Append($"{DateTime.UtcNow.ToShortTimeString()}-");
                        break;
                    case CustomIdElementEnum.UIntSequence:
                        sb.Append($"{incrementValue + 1}-");
                        break;
                }
            }
            return sb.ToString().Trim('-');
        }

        private int RandomNumberWithMinValue(int minExclusive, int maxExclusive)
        {
            if (minExclusive > maxExclusive)
                return 0;

            int result;
            do
                result = RandomNumberGenerator.GetInt32(maxExclusive);
            while (result <= minExclusive);

            return result;
        }
    }
}
