using EventFeed;
using LoyaltyProgram.EventFeed;
using Nancy;
using Nancy.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LoyaltyProgram.UnitTests
{
    public class EventFeed_should
    {
        private Browser sut;

        public EventFeed_should()
        {
            sut = new Browser(
                with => with.Module<LoyaltyProgramEventFeedModule>().Dependency<IEventStore>(typeof(FakeEventStore)),
                withDefault => withDefault.Accept("application/json"));
        }

        [Fact]
        public async void return_events_when_from_event_store()
        {
            var actual = await sut.Get("/events/", with =>
            {
                with.Query("start", "0");
                with.Query("end", "100");
            });
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.StartsWith("application/json", actual.ContentType);
            Assert.Equal(100, actual.Body.DeserializeJson<IEnumerable<Event>>().Count());
        }

        [Fact]
        public async void return_empty_response_when_there_are_no_more_events()
        {
            var actual = await sut.Get("/events/", with =>
            {
                with.Query("start", "200");
                with.Query("end", "300");
            });

            Assert.Empty(actual.Body.DeserializeJson<IEnumerable<Event>>());
        }
    }
}
