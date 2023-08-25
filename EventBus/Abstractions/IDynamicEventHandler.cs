namespace EventBus.Abstractions
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
