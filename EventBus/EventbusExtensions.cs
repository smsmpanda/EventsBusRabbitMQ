using Microsoft.Extensions.DependencyInjection;

namespace EventBus
{
    public static class EventbusExtensions
    {
        public static void AddEventBusSubscriptionManager(this IServiceCollection services)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
