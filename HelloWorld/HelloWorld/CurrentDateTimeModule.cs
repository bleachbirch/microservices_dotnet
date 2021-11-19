using Nancy;
using Newtonsoft.Json;

namespace HelloMicroservices
{
    public class CurrentDateTimeModule: NancyModule
    {
        public CurrentDateTimeModule()
        {
            Get("/", _ => DateTime.UtcNow);
        }
    }
}
