using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IEventHandle<TEvent> where TEvent : IntegrationEvent
    {
        Task Handle(TEvent @event);
    }
}
