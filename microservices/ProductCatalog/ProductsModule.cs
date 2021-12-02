using Nancy;

namespace ProductCatalog
{
    public class ProductsModule: NancyModule
    {
        public ProductsModule(ProductStore productStore): base("/products")
        {
            Get("", _ =>
            {
                string productIdsString = Request.Query.productIds;
                var productIds = ParseProductIdsFromQueryString(productIdsString);
                var products = productStore.GetProductsByIds(productIds);
                //Вызывающая сторона может кэшировать ответ на сутки
                return Negotiate.WithModel(products).WithHeader("cache-control", "max-age:86400");
            });
        }

        private object ParseProductIdsFromQueryString(string productIdsString)
        {
            //TODO
            return new object();
        }
    }
}
