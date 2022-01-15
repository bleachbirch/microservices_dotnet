using Nancy;

namespace HelloMicroservicePlatfrom
{
    public class Hello: NancyModule
    {
        public Hello(Microservice.Platform.IHttpClientFactory httpClientFactory)
        {
            Get("/", async (_, __) =>
            {
                var client = await httpClientFactory.Create(new Uri("http://otherservice"),
                    "scope_for_other_service");
                var response = await client.GetAsync("/some/path").ConfigureAwait(false);
                return response.StatusCode;
            });
        }
    }
}
