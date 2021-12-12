using Nancy;
using System.Threading;

namespace LoyaltyProgram.IntegrationTests.Mocks
{
    /// <summary>
    /// Имитация ленты событий
    /// </summary>
    public class MockEventFeed : NancyModule
    {
        /// <summary>
        /// Оповещает тест о выполнении опросами микросервиса LoyaltyProgram на предмет появления новых событий
        /// </summary>
        public static AutoResetEvent polled = new AutoResetEvent(false);

        public MockEventFeed()
        {
            Get("/events", _ =>
            {
                polled.Set();
                return new[]
                {
                    new
                    {
                        SequnceNumber = 1,
                        Name = "baz",
                        Content = new
                        {
                            OfferName = "foo",
                            Description = "bar",
                            Item = new {ProductName = "name"}
                        }
                    }
                };
            });
        }
    }
}
