using Dapper;
using Microsoft.Owin;
using Nancy.Owin;
using ShopingCart.Infrastructure;
using System.Data.SqlClient;

namespace ShopingCart
{
    public static class Startup
    {
        public static void Configure(this IApplicationBuilder app)
        {
            app.UseOwin(pipeline =>
            {
                pipeline(next => new MonitoringMiddleware(next, HealthCheck).Invoke);
                pipeline.UseNancy();
            });
        }

        private const string _connectionString =
            "Server=localhost;Database=master;Trusted_Connection=True;InitialCatalog=ShoppingCart;IntegratedSecurity=True";
        private const int _threshold = 1000;

        private static async Task<bool> HealthCheck()
        {
            using(var conn = new SqlConnection(_connectionString))
            {
                //conn.Open();
                var count = (await conn.QueryAsync<int>("select count(ID) from dbo.ShoppingCart")).Single();
                return count > _threshold;
            }
        }
    }
}
