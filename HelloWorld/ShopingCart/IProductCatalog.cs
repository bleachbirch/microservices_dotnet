namespace ShopingCart
{
    public interface IProductCatalog
    {
        Task<IEnumerable<Product>> GetShoppingCartItems(int[] productCatalogIds);
    }
}