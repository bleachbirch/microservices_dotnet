using OWIN.Middleware;

var builder = WebApplication.CreateBuilder(args);
//TODO: ��� ������ ���������, � Net6.0 �� ��� �����������. ���� �����������
builder.WebHost.UseStartup<Startup>();

var app = builder.Build();



app.Run();
