using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IEventHandle<T>;

        void SubscribeDynamic<TH>(string eventName) where TH : IDynamicEventHandler;

        void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicEventHandler;

        void Unsubscribe<T, TH>() where TH : IEventHandle<T> where T : IntegrationEvent;
    }
}
