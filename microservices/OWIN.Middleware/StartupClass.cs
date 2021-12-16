using Microsoft.AspNetCore.Owin;
using Microsoft.Owin;
using System.Linq;

namespace OWIN.Middleware
{
    public class StartupClass
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(buildFunc => buildFunc(next => new ConsoleMiddleware(new AppFunc(next)).Invoke));
        }
    }
}
