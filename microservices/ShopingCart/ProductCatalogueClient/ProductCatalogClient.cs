using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace ShopingCart
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        private static string productCatalogBaseUrl = @"http://private-05cc8-chapter2productcataloguemicroservice.apiary-mock.com";
        private static string getProductPathTemplate = "/products?productIds=[{0}]";
        private static AsyncRetryPolicy exponentialRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds) =>
            await exponentialRetryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false))
            .ConfigureAwait(false);
        
        private async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogService(int[] productCatalogIds)
        {
            var response = await RequestProductFromProductCatalogue(productCatalogIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
        {
            var productResource = string.Format(getProductPathTemplate, string.Join(",", productCatalogueIds));
            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(productCatalogBaseUrl);
                return await httpClient.GetAsync(productResource).ConfigureAwait(false);
            }
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var products = JsonConvert.DeserializeObject<List<ProductCatalogueProduct>>(responseContent);

            return products.Select(product => new ShoppingCartItem
            {
                ProductCatalogId = int.Parse(product.ProductId),
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.Price
            });
        }
    }
}
