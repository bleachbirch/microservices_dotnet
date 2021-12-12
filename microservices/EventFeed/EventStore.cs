using Dapper;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EventFeed
{
    public class EventStore : IEventStore
    {

        private const string _connectionString = "discover://http://127.0.0.1:2113/";
        private readonly IEventStoreConnection _connection = EventStoreConnection.Create(_connectionString);

        public async Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            await _connection.ConnectAsync().ConfigureAwait(false);

            var result = await _connection.ReadStreamEventsForwardAsync(
                "ShoppingCart",
                start: (int)firstEventSequenceNumber,
                count: (int)(lastEventSequenceNumber - firstEventSequenceNumber),
                resolveLinkTos: false).ConfigureAwait(false);

            return result.Events.Select(ev => new
            {
                Content = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(ev.Event.Data)),
                Metadata = JsonConvert.DeserializeObject<EventMetadata>(Encoding.UTF8.GetString(ev.Event.Metadata))
            })
                .Select((ev, index) => new Event
                {
                    SequenceNumber = index + firstEventSequenceNumber,
                    CreationDateTime = ev.Metadata.OccuredAt,
                    Content = ev.Content,
                    EventName = ev.Metadata.EventName
                });
        }

        public async Task Raise(string eventName, object content)
        {
            await _connection.ConnectAsync().ConfigureAwait(false);
            var jsonContent = JsonConvert.SerializeObject(content);
            var metaDataJson = JsonConvert.SerializeObject(new EventMetadata
            {
                OccuredAt = DateTimeOffset.Now,
                EventName = eventName
            });
            var evenData = new EventData(
                Guid.NewGuid(),
                "ShoppingCartEvent",
                isJson: true,
                data: Encoding.UTF8.GetBytes(jsonContent),
                metadata: Encoding.UTF8.GetBytes(metaDataJson));

            await _connection.AppendToStreamAsync("ShoppingCart", ExpectedVersion.Any, evenData);
        }
    }
}
