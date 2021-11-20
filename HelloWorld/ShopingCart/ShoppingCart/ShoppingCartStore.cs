namespace ShopingCart
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private readonly List<ShoppingCart> _store = new List<ShoppingCart>();
        public ShoppingCart Get(int userId) => _store.FirstOrDefault(x => x.UserId == userId);

        public void Save(ShoppingCart shoppingCart)
        {
            if(shoppingCart != null)
            {
                _store.Add(shoppingCart);
            }
        }
    }
}
