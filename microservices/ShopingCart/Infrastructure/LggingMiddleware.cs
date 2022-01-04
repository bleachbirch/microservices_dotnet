using Microsoft.Owin;
using Serilog.Context;
using ILogger = Serilog.ILogger;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using System.Diagnostics;

namespace ShopingCart.Infrastructure
{
    public class CorrelationToken
    {
        public static AppFunc Middleware(AppFunc next)
        {
            return async env =>
            {
                var context = new OwinContext(env);
                var correlationToken = GetCorrelationToken(context);
                context.Set("correlationToken", correlationToken.ToString());
                using (LogContext.PushProperty("CorrelationToken", correlationToken))
                    await next(env);
            };
        }

        private static Guid GetCorrelationToken(OwinContext context)
        {
            if (context.Request.Headers.ContainsKey("Correlation-Token") &&
                Guid.TryParse(context.Request.Headers["Correlation-Token"], out Guid correlationToken))
            {
                return correlationToken;
            }

            return Guid.NewGuid();
        }
    }

    /// <summary>
    /// Журналирование запросов и ответов
    /// </summary>
    public class RequestLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                var context = new OwinContext(env);
                log.Information("Incoming request: {@Method}, {@Path}, {@Headers}",
                    context.Request.Method, context.Request.Path, context.Request.Headers);
                await next(env);
                log.Information("Outcoming response: {@StatusCode}, {@Headers}",
                    context.Response.StatusCode, context.Response.Headers);
            };
        }
    }

    /// <summary>
    /// Изменение длительности выполнения запроса
    /// </summary>
    public class PerformanceLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await next(env);
                stopWatch.Stop();
                var context = new OwinContext(env);
                log.Information("Request: {@Method}, {@Path} executed in {RequestTime:000} ms",
                    context.Request.Method, context.Request.Path, stopWatch.ElapsedMilliseconds);
            };
        }
    }

}
