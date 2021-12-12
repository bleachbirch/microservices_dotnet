using EventFeed;

namespace LoyaltyProgram.EventFeed
{
    public class LoyaltyProgramEventFeedModule : EventFeedModule
    {
        public LoyaltyProgramEventFeedModule(IEventStore eventStore) : base(eventStore)
        {
        }
    }
}
