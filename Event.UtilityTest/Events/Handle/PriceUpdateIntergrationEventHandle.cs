using EventBus.Abstractions;

namespace Event.UtilityTest.Events.Handle
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
