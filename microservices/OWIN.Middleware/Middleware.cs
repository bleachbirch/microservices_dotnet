using Microsoft.Owin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace OWIN.Middleware
{
    public class Middleware
    {
        public static Func<AppFunc, AppFunc> Impl = next => async env =>
        {
            var context = new OwinContext(env);
            if(context.Request.Path.Value == "/test/path")
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                await next(env);
            }
        };
    }
}
