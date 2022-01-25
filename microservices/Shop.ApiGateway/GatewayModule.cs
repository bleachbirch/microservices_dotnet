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
            Get("/productlist", async parameters =>
            {
                var client = await httpClientFactory.Create(new Uri("http://localhost:5100/"), "product_catalog_read");
                var products = GetProductListStub();
                client = await httpClientFactory.Create(new Uri("http://localhost:5200/"), "shopping_cart_write");
                var userId = parameters["userId"];
                return View["Views/productlist", new { ProductList = products }];
            });
        }

        private async Task<IEnumerable<Product>> GetProductList(HttpClient client, Serilog.ILogger logger)
        {
            var response = await client.GetAsync("/products?productIds=1,2,3,4");
            var content = await response?.Content.ReadAsStringAsync();
            logger.Information(content);
            return JsonConvert.DeserializeObject<List<Product>>(content);
        }

        private async Task<IEnumerable<Product>> GetBasketProducts(int userId, HttpClient client, Serilog.ILogger logger) 
        {
            var response = await client.GetAsync($"/shoppingcart/{userId}");
            var content = await response?.Content.ReadAsStringAsync();
            logger.Information(content);
            return JsonConvert.DeserializeObject<ShoppingCart>(content)
                .Items?
                .Select(item => new Product 
                { 
                    ProductId = item.ProductCatalogueId, 
                    ProductName = item.ProductName
                }) ?? new List<Product>();

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
