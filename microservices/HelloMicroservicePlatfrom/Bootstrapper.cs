using Microservice.Platform;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace HelloMicroservicePlatfrom
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            container.UseHttpClientFactory(context);
        }
    }
}
