using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using System.Security.Claims;

namespace LoyaltyProgram
{
    public class Bootstrapper: DefaultNancyBootstrapper
    {
        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration => 
            NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear());

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.OnError += (ctx, ex) =>
            {
                Log("Unhandled", ex);
                return null;
            };
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            context.CurrentUser = context.GetOwinEnvironment()["pos-end-user"] as ClaimsPrincipal;
        }
        private void Log(string message, Exception ex)
        {
            //Отправляем параметры в централизованное хранилище журналов
        }
    }
}
