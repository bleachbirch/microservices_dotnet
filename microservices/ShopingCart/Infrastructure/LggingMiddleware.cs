using Microsoft.Owin;
using Serilog.Context;
using ILogger = Serilog.ILogger;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using System.Diagnostics;

namespace ShopingCart.Infrastructure
{
    public static class LoggingMiddleware
    {
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> UseCorrelationToken(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder)
        {
            builder(next => CorrelationToken.Middleware(next));
            return builder;
        }
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> UseRequestLogging(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, ILogger log)
        {
            builder(next => RequestLogging.Middleware(next, log));
            return builder;
        }
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> UsePerformanceLogging(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, ILogger log)
        {
            builder(next => PerformanceLogging.Middleware(next, log));
            return builder;
        }

    }

    internal class CorrelationToken
    {
        internal static AppFunc Middleware(AppFunc next)
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
    internal class RequestLogging
    {
        internal static AppFunc Middleware(AppFunc next, ILogger log)
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
    internal class PerformanceLogging
    {
        internal static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await next(env);
                stopWatch.Stop();
                var context = new OwinContext(env);
                log.Information("Request: {@Method}, {@Path} executed in {@RequestTime:000} ms",
                    context.Request.Method, context.Request.Path, stopWatch.ElapsedMilliseconds);
            };
        }
    }

}
