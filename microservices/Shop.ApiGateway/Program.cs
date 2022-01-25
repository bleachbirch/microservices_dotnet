
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Nancy.Owin;
using Shop.ApiGateway;

var app = ConfigureBuilder().Build();

app.Configure();

app.Run();

WebApplicationBuilder ConfigureBuilder()
{
    var builder = WebApplication.CreateBuilder();

    builder.Services.Configure<KestrelServerOptions>(config =>
    {
        config.AllowSynchronousIO = true;
    });

    return builder;
}
