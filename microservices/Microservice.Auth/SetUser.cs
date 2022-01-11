using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using System.Security.Claims;

namespace Microservice.Auth
{
    public class SetUser : IRequestStartup
    {
        public void Initialize(IPipelines pipelines, NancyContext context)
        {
            context.CurrentUser = context.GetOwinEnvironment()[Constants.END_USER_HEADER] as ClaimsPrincipal;
        }
    }
}
