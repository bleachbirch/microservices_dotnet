
using LoyaltyProgramEventConsumer;
using System.ServiceProcess;

public static class Program
{
    public static void Main(string[] args)
    {
        ServiceBase.Run(new EventConsumerService());
    }
}