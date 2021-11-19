namespace ShopingCart
{
    public class ShoppingCart
    {
        public int UserId { get; set; }

        public IEnumerable<Product> Items { get; set; } = Enumerable.Empty<Product>();

        public void AddItems(IEnumerable<Product> shoppingCartItems, IEventStore eventStore)
        {
            throw new NotImplementedException();
        }

        public void RemoveItems(IEnumerable<int> shoppingCartItems, IEventStore eventStore)
        {
            throw new NotImplementedException();
        }
    }
}