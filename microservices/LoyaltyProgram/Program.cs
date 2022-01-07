using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
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
    pipeline(next => env =>
    {
        var context = new OwinContext(env);
        if (context.Request.Headers.ContainsKey("pos-end-user"))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Читаем и проверяем маркер ID 
            var userPrincipal = tokenHandler.ValidateToken(
                context.Request.Headers["pos-end-user"],
                new TokenValidationParameters(),
                out SecurityToken token);
            // Создаем пользователя на основе маркераа ID  и добавляем его в окружение OWIN
            context.Set("pos-end-user", userPrincipal);
        }
        return next(env);
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
