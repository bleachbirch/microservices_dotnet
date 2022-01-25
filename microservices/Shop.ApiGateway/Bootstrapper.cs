using Microservice.Platform;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.TinyIoc;

namespace Shop.ApiGateway
{
    internal class Bootstrapper : DefaultNancyBootstrapper
    {
        private Serilog.ILogger _logger;

        public Bootstrapper(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register(_logger);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            container.UseHttpClientFactory(context);
        }
    }
}