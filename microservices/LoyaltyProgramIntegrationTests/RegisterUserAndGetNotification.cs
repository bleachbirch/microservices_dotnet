using LoyaltyProgram.IntegrationTests.Mocks;
using LoyaltyProgram.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nancy.Hosting.Self;
using Nancy.Owin;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace LoyaltyProgram.IntegrationTests
{
    public class RegisterUserAndGetNotification : IDisposable
    {
        private IWebHost hostForMockEndpoints;
        private Thread thread;
        private Process eventConsumer;
        private Process api;

        public RegisterUserAndGetNotification()
        {
            StartLoyaltyProgram();
            StartFakeEndpoints();
        }

        private void StartLoyaltyProgram()
        {
            StartEventConsumer();
            StartLoyaltyProgramApi();
        }

        private void StartLoyaltyProgramApi()
        {
            //Настройка для выполнения команды dotnetrun из каталога проекта LoyaltyProgram
            var apiInfo = new ProcessStartInfo("dotnet.exe")
            {
                Arguments = "run",
                WorkingDirectory = "../../../../LoyaltyProgram"
            };
            //Запуск процесса LoyaltyProgram
            api = Process.Start(apiInfo) ?? throw new InvalidDataException();
        }

        private void StartEventConsumer()
        {
            var eventConsumerInfo = new ProcessStartInfo("dotnet.exe")
            {
                Arguments = "run localhost:5001",
                WorkingDirectory = "../../../../LoyaltyProgramEventConsumer"
            };
            eventConsumer = Process.Start(eventConsumerInfo) ?? throw new InvalidDataException();
        }

        private void StartFakeEndpoints()
        {
            hostForMockEndpoints = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<FakeStartup>()
                .UseUrls("http://localhost:5001")
                .Build();

            thread = new Thread(() => hostForMockEndpoints.Run());
            thread.Start();
        }

        public void Dispose()
        {
            eventConsumer?.Dispose();
            api?.Dispose();
        }

        [Fact]
        public void Scenraio()
        {
            //Выполнение HTTP-запроса для регистрации пользователя
            RegisterNewUser();
            //Ожидание выполнения микросервисом LoyaltyProgram опроса на предмет появления новых событий
            WaitForConsumerToReadSpecialOffersEvents();
            //Контроль выполнения запроса к конечной точке уведомлений
            AssertNotificationWasSent();
        }

        private void AssertNotificationWasSent()
        {
            Assert.True(MockNotifications.NotificationWasSent);
        }

        private void WaitForConsumerToReadSpecialOffersEvents()
        {
            Assert.True(MockEventFeed.polled.WaitOne(30000));
            Thread.Sleep(100);
        }

        private async void RegisterNewUser()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:5000");
                var response = await httpClient.PostAsync("/users/",
                    new StringContent(
                        JsonConvert.SerializeObject(new LoyaltyProgramUser()),
                        Encoding.UTF8,
                        "application/json")).ConfigureAwait(false);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }
    }

    public class FakeStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(buildFunc => buildFunc.UseNancy());
        }
    }
}