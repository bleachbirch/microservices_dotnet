using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Nancy.Owin;
using System.IdentityModel.Tokens.Jwt;

var app = ConfigureBuilder().Build();

app.UseOwin(pipeline => pipeline.UseNancy())
    .UseAuthentication();

app.Run();

WebApplicationBuilder ConfigureBuilder()
{
    var builder = WebApplication.CreateBuilder();

    builder.Services.Configure<KestrelServerOptions>(config =>
    {
        config.AllowSynchronousIO = true;
    });

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    builder.Services
        .AddAuthentication()
        .AddCookie("Cookies");

    var oidcOptions = new OpenIdConnectOptions
    {
        SignInScheme = "Cookies",
        Authority = "http://localhost:5001",
        RequireHttpsMetadata = false,
        ClientId = "web",
        ResponseType = "id_token token",
        GetClaimsFromUserInfoEndpoint = true,
        SaveTokens = true
    };
    oidcOptions.Scope.Clear();
    oidcOptions.Scope.Add("openid");
    oidcOptions.Scope.Add("profile");
    oidcOptions.Scope.Add("api1");

    return builder;
}
