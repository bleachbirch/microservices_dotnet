using Microsoft.Owin;
using System.Security.Claims;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Microservice.Auth
{
    internal static class AuthorizationMiddleware
    {
        /// <summary>
        /// Проверяет все входящие запросы на наличие требуемой области действия
        /// </summary>
        /// <param name="next">Следующий элемент конвейера</param>
        /// <param name="requiredScope">Требуемая область действия</param>
        /// <returns></returns>
        internal static AppFunc Middleware(AppFunc next, string requiredScope)
        {
            return env =>
            {
                var context = new OwinContext(env);
                var principal = context.Request.User as ClaimsPrincipal;
                if (principal is not null && principal.HasClaim("scope", requiredScope))
                {
                    return next(env);
                }
                context.Response.StatusCode = 403;
                return Task.FromResult(0);
            };
        }
    }
}
