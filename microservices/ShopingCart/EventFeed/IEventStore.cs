using ShopingCart.EventFeed;

namespace ShopingCart
{
    public interface IEventStore
    {
        Task Raise(string message, object parametrs);
        Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
    }
}