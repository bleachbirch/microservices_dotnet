using Microsoft.Owin;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using System.Security.Claims;

namespace ApiGateway
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            var correlationToken = owinContext.Environment["correlationToken"] as string;
            // Читаем пользователя из окружения OWIN
            var principal = owinContext.Request.User as ClaimsPrincipal;
            // Читаем маркер ID
            var idToken = principal.FindFirst("id_token");
            // Передаем маркер в фабрику
            container.Register<IHttpClientFactory>(new HttpClientFactory(idToken.Value, correlationToken));
        }

    }
}
