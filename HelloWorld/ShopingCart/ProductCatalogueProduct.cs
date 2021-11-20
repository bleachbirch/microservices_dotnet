namespace ShopingCart
{
    public class ProductCatalogueProduct
    {
        public string ProductId { get; set; }
        public Price Price { get; internal set; }
        public string ProductDescription { get; internal set; }
        public string ProductName { get; internal set; }
    }
}