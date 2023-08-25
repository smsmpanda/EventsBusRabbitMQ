using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    /// <summary>
    /// 管理事件与事件处理器订阅关系
    /// </summary>
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string>? OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => _handlers.Keys.Any();

        public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandle<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(T), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        public void Clear() => _handlers.Clear();

        public string GetEventKey<T>() => typeof(T).Name;

        public Type GetEventTypeByName(string eventName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);


        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);

            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo? handlerToRemove)
        {
            if (handlerToRemove != null)
            {
                _handlers[eventName].Remove(handlerToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        private SubscriptionInfo? FindDynamicSubscriptionToRemove<TH>(string eventName)
          where TH : IDynamicEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo? DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandle<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
             where T : IntegrationEvent
             where TH : IEventHandle<T>
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }
    }
}
