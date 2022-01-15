using HelloMicroservicePlatfrom;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var app = ConfigureBuilder().Build();

app.MapGet("/", () => "Hello World!");
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

