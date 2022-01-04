﻿using Dapper;
using Microsoft.Owin;
using System.Data.SqlClient;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using PathString = Microsoft.Owin.PathString;

namespace ShopingCart.Infrastructure
{
    public static class MonitoringMiddleware
    {
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> UseMonitoring(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder)
        {
            builder(next => new Monitoring(next, HealthCheck).Invoke);
            return builder;
        }

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
    }

    internal class Monitoring
    {
        private AppFunc _next;
        private Func<Task<bool>> _healthCheck;

        private static readonly PathString _monitorPath = new PathString("/_monitor");
        private static readonly PathString _monitorShallowPath = new PathString("/_monitor/shallow");
        private static readonly PathString _monitorDeepPath = new PathString("/_monitor/deep");

        public Monitoring(AppFunc next, Func<Task<bool>> healthCheck)
        {
            _next = next;
            _healthCheck = healthCheck;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            if (context.Request.Path.StartsWithSegments(_monitorPath))
            {
                return HandleMonitorEndpoint(context);
            }

            return _next(env);
        }

        private Task HandleMonitorEndpoint(OwinContext context)
        {
            if (context.Request.Path.StartsWithSegments(_monitorShallowPath))
            {
                return ShallowEndpoint(context);
            }

            if (context.Request.Path.StartsWithSegments(_monitorDeepPath))
            {
                return DeepEndpoint(context);
            }

            return Task.FromResult(0);
        }

        private async Task DeepEndpoint(OwinContext context)
        {
            context.Response.StatusCode = await _healthCheck() ? 204 : 503;
        }

        private Task ShallowEndpoint(OwinContext context)
        {
            context.Response.StatusCode = 204;
            return Task.FromResult(0);
        }
    }
}
