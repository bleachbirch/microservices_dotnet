using Nancy.Owin;
using Microservice.Logging;
using Serilog;
using Microservice.Platform;

namespace Shop.ApiGateway
{
    public static class Startup
    {
        public static void Configure(this IApplicationBuilder app)
        {
            var logger = ConfigureLogger();
            ConfigureMicroservicePlatform();
            app.UseStaticFiles();
            app.UseOwin()
                .UseMonitoringAndLogging(logger, HealthCheck)
                .UseNancy(opt => opt.Bootstrapper = new Bootstrapper(logger));
        }

        private static void ConfigureMicroservicePlatform()
        {
            MicroservicePlatformHelper.Configure("http://localhost:5001/", "api_gateway", "secret");
        }

        private static Task<bool> HealthCheck()
        {
            return Task.FromResult(true);
        }

        private static Serilog.ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole(
                Serilog.Events.LogEventLevel.Verbose,
                "{NewLine}{TimeStamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
