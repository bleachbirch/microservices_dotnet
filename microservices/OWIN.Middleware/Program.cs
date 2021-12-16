using OWIN.Middleware;

var builder = WebApplication.CreateBuilder(args);
//TODO: Это больше псевдокод, в Net6.0 хз как настраивать. Надо разобраться
builder.WebHost.UseStartup<Startup>();

var app = builder.Build();



app.Run();
