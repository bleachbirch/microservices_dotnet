using Xunit;
using Microsoft.Owin;
using System.Threading.Tasks;
using OWIN.Middleware;

namespace OWIN.Middleware.Tests
{
    public class Middleware_should
    {
        private AppFunc noOp = env => Task.FromResult(0);

        [Fact]
        public void Return404_for_test_path()
        {
            //Формируем окружение OWIN, соответствующее тестовому сценарию
            var context = new OwinContext();
            context.Request.Scheme = "https";
            context.Request.Path = new PathString("/test/path");
            context.Request.Method = "GET";

            //Создаем ковейер с тестируемым промежуточным ПО
            var pipeline = Middleware.Impl(noOp);

            //Выполняем конвейер со сформированным ранее окружением
            pipeline(context.Environment);

            //Проверяем содержимое окружения после выполнения промежуточного ПО
            Assert.Equal(404, context.Response.StatusCode);
        }
    }
}