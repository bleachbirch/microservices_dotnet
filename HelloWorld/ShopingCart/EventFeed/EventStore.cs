using Newtonsoft.Json;
using ShopingCart.EventFeed;

namespace ShopingCart
{
    public class EventStore : IEventStore
    {
        //TODO: storing in database
        private readonly List<Event> _events = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber) =>
            _events
                .Where(e => e.SequenceNumber >= firstEventSequenceNumber && e.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(e => e.SequenceNumber);

        public void Raise(string message, object parametrs)
        {
            _events.Add(new Event { EventName = message, Content = parametrs, SequenceNumber = _events.Count});
            Console.WriteLine($"Event stored: { message } Event parameters: {JsonConvert.SerializeObject(parametrs)}");
        }
    }
}
