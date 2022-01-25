using Nancy;
using Microservice.Platform;
using IHttpClientFactory = Microservice.Platform.IHttpClientFactory;
using Newtonsoft.Json;

namespace Shop.ApiGateway
{
    public class GatewayModule : NancyModule
    {
        public GatewayModule(IHttpClientFactory httpClientFactory, Serilog.ILogger logger)
        {
            Get("/productlist", async _ =>
            {
                var products = GetProductListStub();
                return View["Views/productlist", new { ProductList = products }];
            });
        }

        private async Task<IEnumerable<Product>> GetProductList(IHttpClientFactory clientFactory, Serilog.ILogger logger)
        {
            var client = await clientFactory.Create(new Uri("http://localhost:5100/"), "product_catalog_read");
            var response = await client.GetAsync("/products?productIds=1,2,3,4");
            var content = await response?.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(content);
        }

        private IEnumerable<Product> GetProductListStub()
        {
            return new[]
            {
                new Product {ProductId = 1, ProductName = "T_shirt"},
                new Product {ProductId = 2, ProductName = "Hoodie"},
                new Product {ProductId = 3, ProductName = "Trousers"}
            };
        }
    }
}
