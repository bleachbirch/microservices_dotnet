using Microsoft.Owin;

namespace OWIN.Middleware
{
    public delegate Task AppFunc(IDictionary<string, object> env);
    public class ConsoleMiddleware
    {
        private AppFunc next;

        public ConsoleMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            var method = context.Request.Method;
            var path = context.Request.Path;
            System.Console.WriteLine($"Got request: {method} {path}");
            return next(env);
        }
    }
}
