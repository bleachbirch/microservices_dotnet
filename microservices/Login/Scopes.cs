using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

internal class Scopes
{
    internal static IEnumerable<ApiScope> Get()
    {
        return new[]
        {
            // Стандартные области действия OpenIDConnect
            // Три стандартные области действия для конечных пользователей, определяющих, что включается в 
            // идентификационные маркеры
            new ApiScope(StandardScopes.OpenId),
            new ApiScope(StandardScopes.Profile),
            new ApiScope(StandardScopes.Email),
            // Описание области действия для конечных точек в микросервисе LoyaltyProgram
            new ApiScope("loyalty_program_write", "Loyalty Program write access")
        };
    }
}