using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Owin;
using Nancy.Owin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var app = ConfigureBuilder().Build();

app.UseOwin(pipeline => 
{
    pipeline(next => env =>
    {
        var context = new OwinContext(env);
        var principal = context.Request.User as ClaimsPrincipal;
        if (principal is not null && principal.HasClaim("scope", "loyalty_program_write"))
        {
            return next(env);
        }
        context.Response.StatusCode = 403;
        return Task.FromResult(0);
    });
    pipeline.UseNancy(); 
});

app.Run();

WebApplicationBuilder ConfigureBuilder()
{
    var builder = WebApplication.CreateBuilder();

    builder.Services.Configure<KestrelServerOptions>(config =>
    {
        config.AllowSynchronousIO = true;
    });

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
    builder.Services
        .AddAuthentication()
        // Меняем настройки ASP.Net Core для чтения маркера из входящих запросов
        .AddJwtBearer(options =>
        {
            options.Audience = "http://localhost:5001/resources";
            options.Authority = "http://localhost:5001";
            options.RequireHttpsMetadata = false;
        });
    return builder;
}
