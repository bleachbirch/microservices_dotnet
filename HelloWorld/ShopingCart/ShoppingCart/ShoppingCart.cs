namespace ShopingCart
{
    public class ShoppingCart
    {
        private HashSet<ShoppingCartItem> _items = new HashSet<ShoppingCartItem>();
        public int UserId { get; set; }

        public IEnumerable<ShoppingCartItem> Items => _items;

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach(var item in shoppingCartItems)
            {
                if (_items.Add(item))
                {
                    eventStore.Raise("ShoppingCart Added", new { UserId, item });
                }
            }
        }

        public void RemoveItems(IEnumerable<int> shoppingCartItemIds, IEventStore eventStore)
        {
            foreach(var id in shoppingCartItemIds)
            {
                var item = Items.FirstOrDefault(i => i.ProductCatalogId == id);

                if (item != null && _items.Remove(item))
                {
                    eventStore.Raise("ShoppingCart Removed", new { UserId, item });
                }
            }
        }
    }
}