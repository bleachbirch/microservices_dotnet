using IdentityModel;
using IdentityServer4.Test;
using System.Security.Claims;

internal class Users
{
    internal static List<TestUser> Get()
    {
        return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "818727",
                Username = "alice",
                Password = "alice",
                // Список утверждений для пользователя alice
                Claims = new[]
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.Role, "User"),
                    new Claim(JwtClaimTypes.Id, "1", ClaimValueTypes.Integer64)
                }
            },
            new TestUser
            {
                SubjectId = "88421113", 
                Username = "bob", 
                Password = "bob",
                Claims = new[]
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.Role, "User"),
                    new Claim(JwtClaimTypes.Id, "2", ClaimValueTypes.Integer64)
                }
            }
        };
    }
}