using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Nancy.Owin;
using System.IdentityModel.Tokens.Jwt;
using Microservice.Auth;

var app = ConfigureBuilder().Build();

app.UseOwin(pipeline => 
{
    pipeline
    .UseAuth("loyalty_program_write")
    .UseNancy(); 
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
