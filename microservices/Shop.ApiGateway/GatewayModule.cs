using Nancy;
using Microservice.Platform;
using IHttpClientFactory = Microservice.Platform.IHttpClientFactory;
using Newtonsoft.Json;
using Nancy.ModelBinding;
using System.Text;

namespace Shop.ApiGateway
{
    public class GatewayModule : NancyModule
    {
        private IEnumerable<Product> _products = new List<Product>();
        public GatewayModule(IHttpClientFactory httpClientFactory, Serilog.ILogger logger)
        {
            Get("/productlist", async parameters =>
            {
                var client = await httpClientFactory.Create(new Uri("http://localhost:5100/"), "product_catalog_read");
                _products = GetProductListStub();
                client = await httpClientFactory.Create(new Uri("http://localhost:5200/"), "shopping_cart_write");
                var userId = parameters["userId"];
                var basketProducts = GetBasketProducts(userId, client, logger);
                return View["Views/productlist", new { ProductList = _products, BasketProducts = basketProducts}];
            });

            Post("/shoppingcart/{userId}/items", async parametrs =>
            {
                var productId = this.Bind<int>();
                var userId = (int)parametrs["userId"];

                var client = await httpClientFactory.Create(new Uri("http://localhost:5200/"),
                    "shopping_cart_write");
                var response = await client.PostAsync("/shoppingcart/{userId}/items",
                    new StringContent(JsonConvert.SerializeObject(new[] { productId }),
                    System.Text.Encoding.UTF8,
                    "application/json"));
                var content = await response?.Content.ReadAsStringAsync();
                var basketProducts = GetBasketProductsFromResponse(content);
                logger.Information("{@basket}", basketProducts);
                return View["View/productList", new { ProductList = _products, BasketProducts = basketProducts }];
            });

            Delete("/shoppingcart/{userId}", async parameters =>
            {
                var productId = this.Bind<int>();
                var userId = (int)parameters["userId"];

                var client = await httpClientFactory.Create(new Uri("http://localhost:5200/"),
                    "shopping_cart_write");
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(client.BaseAddress, $"/shoppingcart/{userId}/items"),
                    Content = new StringContent(JsonConvert.SerializeObject(new[] { productId }), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                var content = await response?.Content.ReadAsStringAsync();
                var basketProducts = GetBasketProductsFromResponse(content);
                logger.Information("{@basket}", basketProducts);
                return View["View/productList", new { ProductList = _products, BasketProducts = basketProducts }];
            });
        }

        private IEnumerable<Product> GetBasketProductsFromResponse(string content)
        {
            return JsonConvert.DeserializeObject<ShoppingCart>(content)
                .Items?
                .Select(item => new Product
                {
                    ProductId = item.ProductCatalogueId,
                    ProductName = item.ProductName
                }) ?? new List<Product>();
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
            return GetBasketProductsFromResponse(content);

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
