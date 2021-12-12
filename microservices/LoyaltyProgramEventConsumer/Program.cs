
using LoyaltyProgramEventConsumer;
using System.ServiceProcess;

public static class Program
{
    public static void Main(string[] args) => new EventConsumerService().Entry(args);
}