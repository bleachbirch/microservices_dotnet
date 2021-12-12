using Nancy;
using Nancy.Testing;
using System.Threading.Tasks;
using Xunit;

namespace LoyaltyProgram.UnitTests
{
    public class TestModule_should
    {
        public class TestModule : NancyModule
        {
            public TestModule()
            {
                Get("/", _ => 200);
            }
        }

        [Fact]
        public async Task Respond_Ok_To_Request_To_Root()
        {
            //Указание для загрузчика Nancy тестируемого модуля
            //sut - system under test (тестируемая система)
            var sut = new Browser(with => with.Module<TestModule>());
            var actual = await sut.Get("/");
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }
    }
}
