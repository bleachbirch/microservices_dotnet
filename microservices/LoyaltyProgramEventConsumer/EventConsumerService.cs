using System.ServiceProcess;

namespace LoyaltyProgramEventConsumer
{
    public class EventConsumerService: ServiceBase
    {
        private EventSubcriber _subscriber;

        private readonly string _loyaltyProgramHost = "localhost:5000";

        public EventConsumerService()
        {
            _subscriber = new EventSubcriber(_loyaltyProgramHost);
        }

        public void Entry(string[] args)
        {
            _subscriber = new EventSubcriber(args[0]);
            if (args.Length > 1 && args[1].Equals("--service"))
                Run(this);
            else
            {
                OnStart(null);
                Console.ReadLine();
            }
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
