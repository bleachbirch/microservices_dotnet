using Microsoft.Owin;
using Serilog.Context;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

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
}
