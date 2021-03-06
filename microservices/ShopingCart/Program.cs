
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Nancy.Owin;
using ShopingCart;

var app = ConfigureBuilder().Build();

//app.UseOwin(pipeline => pipeline.UseNancy());
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
