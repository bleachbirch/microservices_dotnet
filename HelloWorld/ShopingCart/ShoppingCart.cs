namespace ShopingCart
{
    public class ShoppingCart
    {
        public int UserId { get; set; }

        public IEnumerable<ShoppingCartItem> Items { get; set; } = Enumerable.Empty<ShoppingCartItem>();

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            throw new NotImplementedException();
        }

        public void RemoveItems(IEnumerable<int> shoppingCartItems, IEventStore eventStore)
        {
            throw new NotImplementedException();
        }
    }
}