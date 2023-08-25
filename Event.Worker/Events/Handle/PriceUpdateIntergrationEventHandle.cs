using Event.Worker.Events;
using EventBus.Abstractions;

namespace Event.Worker.Handle
{
    public class PriceUpdateIntergrationEventHandle : IEventHandle<PriceUpdateIntergrationEvent>
    {
        public Task Handle(PriceUpdateIntergrationEvent @event)
        {
            Console.WriteLine(@event.EventId);
            return Task.CompletedTask;
        }
    }
}
