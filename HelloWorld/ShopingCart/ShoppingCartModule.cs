using Nancy;
using Nancy.ModelBinding;

namespace ShopingCart
{
    public class ShoppingCartModule: NancyModule
    {
        public ShoppingCartModule(IShoppingCartStore shoppingCartStore,
            IProductCatalogClient productCatalog,
            IEventStore eventStore) 
            : base("/shoppingcart")
        {
            Get("/{userId:int}", parametrs =>
            {
                var userId = (int)parametrs.userId;
                return shoppingCartStore.Get(userId);
            });

            Post("/{userId:int}/items", async (parametrs, _) =>
            {
                var productCatalogIds = this.Bind<int[]>();
                var userId = (int)parametrs.userId;

                var shoppingCart = shoppingCartStore.Get(userId);
                var shoppingCartItems = await productCatalog.GetShoppingCartItems(productCatalogIds)
                .ConfigureAwait(false);
                shoppingCart.AddItems(shoppingCartItems, eventStore);
                shoppingCartStore.Save(shoppingCart);

                return shoppingCart;
            });

            Delete("/{userId:int}/items", (parametrs) =>
            {
                var productCatalogIds = this.Bind<int[]>();
                var userId = (int)parametrs.userId;

                var shoppingCart = shoppingCartStore.Get(userId);
                shoppingCart.RemoveItems(productCatalogIds, eventStore);
                shoppingCartStore.Save(shoppingCart);

                return shoppingCart;

            });
        }
    }
}
