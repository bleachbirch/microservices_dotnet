using Nancy;

namespace ShopingCart.EventFeed
{
    public class EventFeedModule: NancyModule
    {
        public EventFeedModule(IEventStore eventStore): base("/events")
        {
            Get("/", _ =>
            {
                long firstEventSequenceNumber = 0, lastEventSequenceNumber = 0;

                long.TryParse(Request.Query.start.Value, out firstEventSequenceNumber);
                long.TryParse(Request.Query.end.Value, out lastEventSequenceNumber);

                return eventStore.GetEvents(firstEventSequenceNumber, lastEventSequenceNumber);
            });
        }
    }
}
