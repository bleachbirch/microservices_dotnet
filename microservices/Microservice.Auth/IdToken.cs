using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using System.IdentityModel.Tokens.Jwt;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Microservice.Auth
{
    internal static class IdToken
    {
        /// <summary>
        /// Считывает идентификатор конечного пользователя из заголовка
        /// </summary>
        /// <param name="next">Следующий элемент конвейера</param>
        /// <returns></returns>
        internal static AppFunc Middleware(AppFunc next)
        {
            return env =>
            {
                var context = new OwinContext(env);
                if (context.Request.Headers.ContainsKey(Constants.END_USER_HEADER))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    // Читаем и проверяем маркер ID 
                    var userPrincipal = tokenHandler.ValidateToken(
                        context.Request.Headers[Constants.END_USER_HEADER],
                        new TokenValidationParameters(),
                        out SecurityToken token);
                    // Создаем пользователя на основе маркераа ID  и добавляем его в окружение OWIN
                    context.Set(Constants.END_USER_HEADER, userPrincipal);
                }
                return next(env);
            };
        }
    }
}
