using BuildFunc = System.Action<System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;

namespace Microservice.Auth
{
    public static class Extensions
    {
        public static BuildFunc UseAuth(this BuildFunc pipeline, string requiredScope)
        {
            pipeline(next => AuthorizationMiddleware.Middleware(next, requiredScope));
            pipeline(next => IdToken.Middleware(next));
            return pipeline;
        } 
    }
}
