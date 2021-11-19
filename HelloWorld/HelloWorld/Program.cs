using Microsoft.AspNetCore.Server.Kestrel.Core;
using Nancy.Owin;

var app = ConfigureBuilder().Build();
app.UseOwin(builder =>
{
    builder(next => env =>
    {
        Console.WriteLine("GotRequest");
        return next(env);
    });
    builder.UseNancy();
});
app.Run();

WebApplicationBuilder ConfigureBuilder(string[] args = null)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.AllowSynchronousIO = true;
    });

    return builder;
}