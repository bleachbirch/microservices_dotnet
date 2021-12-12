using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoyaltyProgram.IntegrationTests.Mocks
{
    /// <summary>
    /// Имитационная консная точка, регистрирущая обращения к ней
    /// </summary>
    public class MockNotifications : NancyModule
    {
        public static bool NotificationWasSent = false;

        public MockNotifications()
        {
            Get("/notify", _ =>
            {
                NotificationWasSent = true;
                return 200;
            });
        }
    }
}
