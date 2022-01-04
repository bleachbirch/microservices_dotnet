using Nancy.Owin;
using ShopingCart.Infrastructure;

namespace ShopingCart
{
    public static class Startup
    {
        public static void Configure(this IApplicationBuilder app)
        {
            app.UseOwin(pipeline =>
            {
                pipeline
                .UseMonitoring()
                .UseNancy();
            });
        }

    }
}
