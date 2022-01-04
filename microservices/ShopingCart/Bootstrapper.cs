using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ShopingCart
{
    public class Bootstrapper: DefaultNancyBootstrapper
    {
        // Регистратор
        private readonly ILogger _logger;

        public Bootstrapper(ILogger logger)
        {
            _logger = logger;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            // Отмечает регистратор в контейнере Nancy
            container.Register(_logger);
            container.Register<IHttpClientFactory>(new HttpClientFactory(Guid.NewGuid().ToString()));
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            // Получаем маркер корреляции из окружения
            var correlationToken = context.Environment.ContainsKey("correlationToken") ?
                context.Environment["correlationToken"] as string :
                Guid.NewGuid().ToString();

            // Внедряем маркер корреляции в HttpClientFactory
            container.Register<IHttpClientFactory>(new HttpClientFactory(correlationToken));
        }
    }
}
