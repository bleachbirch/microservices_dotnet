using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging.Console;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

public class Startup
{
    private readonly IHostingEnvironment environment;

    public Startup(IHostingEnvironment env)
    {
        environment = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var cert = new X509Certificate2(Path.Combine(environment.ContentRootPath, "idsrv4test.pfx"), "idsrv3test");

        // Конфигурация ASP.Net Core, от которой зависит IdentityServer
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services
            // Настраиваем IdentityServer
            .AddIdentityServer()
            // Используем сертификат для подписывания маркеров
            .AddSigningCredential(cert)
            // Добавляем хранящие в оперативной памяти версии всего, что нужно IdentityServer
            .AddInMemoryClients(Clients.Get())
            .AddInMemoryApiScopes(Scopes.Get())
            .AddTestUsers(Users.Get());

        // Отключаем EndpointRouting, иначе UseMvcWithDefaultRoute работать не будет
        services.AddMvc(opt => opt.EnableEndpointRouting = false);

        services
            .AddAuthentication()
            .AddCookie("Temp");

        services.AddLogging(loggerBuilder => loggerBuilder
            .AddConsole()
            .AddDebug()
            .SetMinimumLevel(LogLevel.Trace)
        );
    }

    public void Configure(IApplicationBuilder app)
    {
        // Запускаем IdentityServer
        app.UseIdentityServer()
            .UseStaticFiles()
            // Добавляем необходимую IdentityServer маршрутизацию
            .UseMvcWithDefaultRoute();
    }
}