namespace ShopingCart
{
    public interface IShoppingCartStore
    {
        /// <summary>
        /// Возвращает корзину клиента по его Id 
        /// </summary>
        /// <param name="userId">Id клиента</param>
        /// <returns>корзина</returns>
        ShoppingCart Get(int userId);
        void Save(ShoppingCart shoppingCart);
    }
}