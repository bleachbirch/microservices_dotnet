namespace ShopingCart
{
    public interface IShoppingCartStore
    {
        /// <summary>
        /// Возвращает корзину клиента по его Id 
        /// </summary>
        /// <param name="userId">Id клиента</param>
        /// <returns>корзина</returns>
        Task<ShoppingCart> Get(int userId);
        Task Save(ShoppingCart shoppingCart);
    }
}