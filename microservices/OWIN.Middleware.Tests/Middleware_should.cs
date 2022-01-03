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
            //��������� ��������� OWIN, ��������������� ��������� ��������
            var context = new OwinContext();
            context.Request.Scheme = "https";
            context.Request.Path = new PathString("/test/path");
            context.Request.Method = "GET";

            //������� ������� � ����������� ������������� ��
            var pipeline = Middleware.Impl(noOp);

            //��������� �������� �� �������������� ����� ����������
            pipeline(context.Environment);

            //��������� ���������� ��������� ����� ���������� �������������� ��
            Assert.Equal(404, context.Response.StatusCode);
        }
    }
}