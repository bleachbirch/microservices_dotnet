using Nancy.Owin;
using Serilog;
using Serilog.Events;
using ShopingCart.Infrastructure;
using ILogger = Serilog.ILogger;

namespace ShopingCart
{
    public static class Startup
    {
        public static void Configure(this IApplicationBuilder app)
        {
            var log = ConfigureLogger();

            app.UseOwin(pipeline =>
            {
                pipeline
                .UseCorrelationToken()
                .UseRequestLogging(log)
                .UsePerformanceLogging(log)
                .UseMonitoring()
                .UseNancy(opt => opt.Bootstrapper = new Bootstrapper(log));
            });
        }

        private static ILogger ConfigureLogger()
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
