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
                //pipeline(next => GlobalErrorLogging.Middleware(next, log));
                pipeline(next => CorrelationToken.Middleware(next));
                pipeline(next => RequestLogging.Middleware(next, log));
                pipeline(next => PerformanceLogging.Middleware(next, log));
                pipeline
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
