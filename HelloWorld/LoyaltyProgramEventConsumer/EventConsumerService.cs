using System.ServiceProcess;

namespace LoyaltyProgramEventConsumer
{
    public class EventConsumerService: ServiceBase
    {
        private EventSubcriber subscriber;

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
