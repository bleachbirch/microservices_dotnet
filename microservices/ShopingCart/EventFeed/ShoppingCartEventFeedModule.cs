using EventFeed;

namespace ShopingCart.EventFeed
{
    public class ShoppingCartEventFeedModule : EventFeedModule
    {
        public ShoppingCartEventFeedModule(IEventStore eventStore) : base(eventStore)
        {
        }
    }
}
