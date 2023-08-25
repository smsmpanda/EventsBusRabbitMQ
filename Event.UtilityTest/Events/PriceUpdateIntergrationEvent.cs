using EventBus.Events;

namespace Event.UtilityTest.Events
{
    /// <summary>
    /// 数据变动事件
    /// </summary>
    public class PriceUpdateIntergrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public float OriginalPrice { get; set; }

        /// <summary>
        /// 现价
        /// </summary>
        public float CurrentPrice { get; set; }
    }
}
