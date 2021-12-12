namespace EventFeed
{
    public interface IEventStore
    {
        Task Raise(string message, object parametrs);
        Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
    }
}