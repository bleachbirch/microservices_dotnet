using Microservice.Logging;
using Microservice.Auth;
using Nancy.Owin;
using Serilog;
using Serilog.Events;

namespace HelloMicroservicePlatfrom
{
    public static class Startup
    {
        public static void Configure(this IApplicationBuilder app)
        {
            app.UseOwin()
                .UseMonitoringAndLogging(ConfigureLogger(), HealthCheck)
                .UseAuth("test-scope")
                .UseNancy();
        }

        private static Task<bool> HealthCheck()
        {
            return Task.FromResult(true);
        }

        private static Serilog.ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                //добавление маркера корреляции
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
