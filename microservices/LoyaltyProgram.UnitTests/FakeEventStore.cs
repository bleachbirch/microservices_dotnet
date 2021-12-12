using EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyProgram.UnitTests
{
    internal class FakeEventStore : IEventStore
    {
        public async Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            if (firstEventSequenceNumber > 0)
                return Enumerable.Empty<Event>();

            return Enumerable
                .Range((int)firstEventSequenceNumber, (int)(lastEventSequenceNumber - firstEventSequenceNumber))
                .Select(i => new Event
                {
                    SequenceNumber = i,
                    CreationDateTime = DateTimeOffset.Now,
                    EventName = "some event",
                    Content = new Object()
                });
        }

        public Task Raise(string message, object parametrs)
        {
            throw new NotImplementedException();
        }
    }
}
