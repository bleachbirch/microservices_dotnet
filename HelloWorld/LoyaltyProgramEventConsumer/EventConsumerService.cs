using System.ServiceProcess;

namespace LoyaltyProgramEventConsumer
{
    public class EventConsumerService: ServiceBase
    {
        private readonly EventSubcriber _subscriber;
        private readonly string _loyaltyProgramHost = "localhost:5000";

        public EventConsumerService()
        {
            _subscriber = new EventSubcriber(_loyaltyProgramHost);
        }

        protected override void OnStart(string[] args)
        {
            _subscriber.Start();
        }

        protected override void OnStop()
        {
            _subscriber.Stop();
        }
    }
}
