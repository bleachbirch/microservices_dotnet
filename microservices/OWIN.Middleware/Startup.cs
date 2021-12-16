using Microsoft.AspNetCore.Owin;
using Microsoft.Owin;
using System.Linq;

namespace OWIN.Middleware
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(buildFunc => buildFunc(next => env =>
            {
                var context = new OwinContext(env);
                var method = context.Request.Method;
                var path = context.Request.Path;
                System.Console.WriteLine($"Got request: {method} {path}");
                return next(env);
            }));
        }
    }
}
