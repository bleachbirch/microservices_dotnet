using IdentityServer4.Models;
using static IdentityModel.OidcConstants;
using GrantTypes = IdentityServer4.Models.GrantTypes;

internal class Clients
{
    internal static IEnumerable<Client> Get()
    {
        return new Client[]
        {
            new Client
            {
                ClientName = "API Gateway",
                ClientId = "api_gateway",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                // Области действия, включаемые в маркеы доступа для микросервиса API Gateway
                AllowedScopes = new List<string>
                {
                    "loyalty_program_write"
                },
                AllowedGrantTypes = GrantTypes.ClientCredentials
            },
            new Client
            {
                ClientId = "web",
                ClientName = "Web Client",
                RedirectUris = new List<string>
                {
                    "http://localhost:5003/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    "http://localhost:5003/"
                },
                AllowedScopes = new List<string>
                {
                    StandardScopes.OpenId,
                    StandardScopes.Email,
                    StandardScopes.Profile
                }
            }
        };
    }
}