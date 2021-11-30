using Nancy.Owin;

namespace HelloMicroservices
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(builder => builder.UseNancy());
        }
    }
}
