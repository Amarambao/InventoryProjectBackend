using CommonLayer.Models.Dto.Message;

namespace API.Notifications
{
    public interface IChatEventsPublisher
    {
        Task MessageCreatedAsync(Guid inventoryId, MessageGetDto dto);
        Task MessagesDeletedAsync(Guid inventoryId, IEnumerable<DateTime> writtenAtsUtc);
    }
}
