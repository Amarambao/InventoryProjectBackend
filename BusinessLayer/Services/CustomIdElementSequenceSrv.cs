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
        private readonly IStoredItemsRepo _storedItemsRepo;

        public CustomIdElementSequenceSrv(
            ICustomIdElementSequenceRepo customIdElementSequenceRepo,
            IStoredItemsRepo storedItemsRepo)
        {
            _customIdElementSequenceRepo = customIdElementSequenceRepo;
            _storedItemsRepo = storedItemsRepo;
        }

        public async Task ModifyCustomIdSequenceAsync(Guid inventoryId, Guid itemId, List<CustomIdElementDto> sequenceDto)
        {
            var filteredSequence = new List<CustomIdElementDto>();
            var seenUIntSequence = false;

            foreach (var dto in sequenceDto)
            {
                if (dto.ElementType == CustomIdElementEnum.UIntSequence)
                    if (seenUIntSequence)
                        continue; 
                    seenUIntSequence = true;

                filteredSequence.Add(dto);
            }

            var existingSequence = (await _customIdElementSequenceRepo.GetItemSequenceAsync(inventoryId, itemId))
                .ToDictionary(s => (s.ElementType, s.FixedTextValue), s => s);

            var updatedEntities = new List<CustomIdElementSequenceEntity>();
            var newEntities = new List<CustomIdElementSequenceEntity>();
            var seenKeys = new HashSet<(CustomIdElementEnum, string?)>();

            for (var i = 0; i < filteredSequence.Count; i++)
            {
                var key = (filteredSequence[i].ElementType, filteredSequence[i].ElementType == CustomIdElementEnum.FixedText
                    ? filteredSequence[i].FixedTextValue : null);
                seenKeys.Add(key);

                if (existingSequence.TryGetValue(key, out var existing))
                {
                    if (existing.Order != i)
                    {
                        existing.Order = i;
                        updatedEntities.Add(existing);
                    }
                }
                else
                {
                    newEntities.Add(new CustomIdElementSequenceEntity
                    {
                        InventoryId = inventoryId,
                        ItemId = itemId,
                        ElementType = filteredSequence[i].ElementType,
                        FixedTextValue = key.Item2,
                        Order = i
                    });
                }
            }

            var entitiesToRemove = existingSequence
                .Where(kvp => !seenKeys.Contains(kvp.Key))
                .Select(kvp => kvp.Value);

            if (newEntities.Any())
                await _customIdElementSequenceRepo.CreateSequenceAsync(newEntities);

            if (updatedEntities.Any())
                await _customIdElementSequenceRepo.UpdateRangeAsync(updatedEntities);

            if (entitiesToRemove.Any())
                await _customIdElementSequenceRepo.RemoveRangeAsync(entitiesToRemove);
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
            }).ToList();
        }

        public async Task<string> GenerateCustomIdAsync(Guid inventoryId, Guid itemId)
        {
            var elements = await _customIdElementSequenceRepo.GetItemSequenceAsync(inventoryId, itemId);
            var sb = new StringBuilder();

            var UIntSequenceValue = $"{await _storedItemsRepo.GetMaxUIntStoredAsync(inventoryId, itemId)}-";
            foreach (var element in elements)
            {
                switch (element.ElementType)
                {
                    case CustomIdElementEnum.FixedText:
                        sb.Append($"{element.FixedTextValue ?? string.Empty}-");
                        break;
                    case CustomIdElementEnum.Random20Bit:
                        sb.Append($"{Convert.ToBase64String(RandomNumberGenerator.GetBytes(20))}-");
                        break;
                    case CustomIdElementEnum.Random32Bit:
                        sb.Append($"{Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))}-");
                        break;
                    case CustomIdElementEnum.Random6Digit:
                        sb.Append($"{RandomNumberGenerator.GetInt32(1000000)}-");
                        break;
                    case CustomIdElementEnum.Random9Digit:
                        sb.Append($"{RandomNumberGenerator.GetInt32(1000000000)}-");
                        break;
                    case CustomIdElementEnum.Guid:
                        sb.Append($"{Guid.NewGuid()}-");
                        break;
                    case CustomIdElementEnum.DateTime:
                        sb.Append($"{DateTime.UtcNow.ToShortTimeString()}-");
                        break;
                    case CustomIdElementEnum.UIntSequence:
                        sb.Append(UIntSequenceValue);
                        break;
                }
            }
            return sb.ToString().Trim('-');
        }
    }
}
