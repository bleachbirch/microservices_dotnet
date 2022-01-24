using Nancy.Owin;
using Microservice.Logging;
using Serilog;
using Microservice.Platform;

namespace Shop.ApiGateway
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var logger = ConfigureLogger();
            ConfigureMicroservicePlatform();
            app.UseStaticFiles();
            app.UseOwin()
                .UseMonitoringAndLogging(logger, HealthCheck)
                .UseNancy(opt => opt.Bootstrapper = new Bootstrapper(logger));
        }

        private void ConfigureMicroservicePlatform()
        {
            MicroservicePlatformHelper.Configure("http://localhost:5001/", "api_gateway", "secret");
        }

        private Task<bool> HealthCheck()
        {
            return Task.FromResult(true);
        }

        private Serilog.ILogger ConfigureLogger()
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
