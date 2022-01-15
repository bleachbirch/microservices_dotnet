using Nancy;
using Nancy.TinyIoc;
using Nancy.Owin;
using Microsoft.Owin;
using Microsoft.AspNetCore.Owin;
using Owin.Types;
using System.Security.Claims;

namespace Microservice.Platform
{
    public static class MicroservicePlatformHelper
    {
        private static string _tokenUrl;
        private static string _clientName;
        private static string _clientSecret;

        public static void Configure(string tokenUrl, string clientName, string clientSecret)
        {
            _tokenUrl = tokenUrl;
            _clientName = clientName;
            _clientSecret = clientSecret;
        }

        public static TinyIoCContainer UseHttpClientFactory(this TinyIoCContainer self, NancyContext context)
        {
            var owinEnvironment = context.GetOwinEnvironment() ?? throw new InvalidOperationException("Owin environment is null");
            var correlationToken = owinEnvironment["correlationToken"] as string;
            owinEnvironment.TryGetValue("owin.RequestUser", out object key);
            var principal = key as ClaimsPrincipal;
            var idToken = principal?.FindFirst("id_token");
            self.Register<IHttpClientFactory>(new HttpClientFactory(_tokenUrl, _clientName, _clientSecret, correlationToken, idToken?.Value));
            return self;
        }

    }
}
