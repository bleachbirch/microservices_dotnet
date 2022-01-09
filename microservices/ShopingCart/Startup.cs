using Dapper;
using Microservice.Logging;
using Nancy.Owin;
using Serilog;
using Serilog.Events;
using System.Data.SqlClient;
using ILogger = Serilog.ILogger;

namespace ShopingCart
{
    public static class Startup
    {
        private const string _connectionString =
            "Server=localhost;Database=master;Trusted_Connection=True;InitialCatalog=ShoppingCart;IntegratedSecurity=True";
        private const int _threshold = 1000;

        private static async Task<bool> HealthCheck()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                //conn.Open();
                var count = (await conn.QueryAsync<int>("select count(ID) from dbo.ShoppingCart")).Single();
                return count > _threshold;
            }
        }

        public static void Configure(this IApplicationBuilder app)
        {
            var log = ConfigureLogger();

            app.UseOwin(pipeline =>
            {
                pipeline
                .UseMonitoringAndLogging(log, HealthCheck)
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
