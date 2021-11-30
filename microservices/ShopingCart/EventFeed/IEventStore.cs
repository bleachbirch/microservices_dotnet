using ShopingCart.EventFeed;

namespace ShopingCart
{
    public interface IEventStore
    {
        void Raise(string message, object parametrs);
        IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
    }
}