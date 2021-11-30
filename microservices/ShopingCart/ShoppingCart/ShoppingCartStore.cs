using System.Data.SqlClient;
using Dapper;

namespace ShopingCart
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string _connectionString =
            "Server=localhost;Database=master;Trusted_Connection=True;InitialCatalog=ShoppingCart;IntegratedSecurity=True";

        private const string _readQuery =
            @"select * from ShoppingCart, ShoppingCartItems 
                where ShoppingCartItems.ShoppingCartId = ID and ShoppingCart.UserId = @UserId";

        private const string _deleteAllForShoppingCartItems =
            @"delete item from ShoppingCartItems item inner join ShoppingCart cart on item.ShoppingCartId = cart.ID
                and cart.UserId=@UserId";

        private const string _addAllForShoppingCartSql =
            @"insert into ShoppingCartItems (ShoppingCartId, ProductCatalogId, ProductName, ProductDesription, Amount, Currency)
                values (@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

        public async Task<ShoppingCart> Get(int userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                //conn.Open();
                var items = await conn.QueryAsync<ShoppingCartItem>(_readQuery, new { UserId = userId });
                return new ShoppingCart { UserId = userId, Items = items };
            }
        }

        public async Task Save(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }

            using (var conn = new SqlConnection(_connectionString))
            using (var transaction = conn.BeginTransaction())
            {
                await conn.ExecuteAsync(_deleteAllForShoppingCartItems, new {UserId = shoppingCart.UserId }, transaction)
                    .ConfigureAwait(false);
                await conn.ExecuteAsync(_addAllForShoppingCartSql, shoppingCart.Items, transaction)
                    .ConfigureAwait(false);
            }
        }
    }
}
