namespace API.Handlers
{
    public interface IEventHandler<TEvent>
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
