using Dapper;
using Newtonsoft.Json;
using ShopingCart.EventFeed;
using System.Data.SqlClient;
using System.Linq;

namespace ShopingCart
{
    public class EventStore : IEventStore
    {

        private const string _connectionString =
            "Server=localhost;Database=master;Trusted_Connection=True;InitialCatalog=ShoppingCart;IntegratedSecurity=True";

        private const string _writeEvent =
            @"insert into EventStore(Name, OccurredAt, Content)
                values (@Name, @OccurredAt, @Content)";

        private const string _readEvent =
            @"select * from EventStore where ID >= @Start and ID <= @End";

        public async Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                return (await conn.QueryAsync<dynamic>(_readEvent, new
                {
                    Start = firstEventSequenceNumber,
                    End = lastEventSequenceNumber
                }).ConfigureAwait(false))
                .Select(row =>
                {
                    var content = JsonConvert.DeserializeObject<Event>(row.Content);
                    return new Event { SequenceNumber = row.ID, Content = content, CreationDateTime = row.OccuredAt, EventName = row.Name };
                });
            }
        }

        public Task Raise(string eventName, object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            using(var conn = new SqlConnection(_connectionString))
            {
                return conn.ExecuteAsync(_writeEvent, new 
                { 
                    Name = eventName, 
                    OccurredAt = DateTimeOffset.Now, 
                    Content = jsonContent 
                });
            }
        }
    }
}
