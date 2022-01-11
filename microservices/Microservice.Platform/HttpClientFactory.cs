using IdentityModel.Client;
using System.Net.Http.Headers;
using Microservice.Auth;

namespace Microservice.Platform
{
    public interface IHttpClientFactory
    {
        Task<HttpClient> Create(Uri uri, string scope);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ClientCredentialsTokenRequest _tokenClient;
        private readonly string _correlationToken;
        private readonly string _idToken;

        public HttpClientFactory(
            string tokenUrl,
            string clientName,
            string clientSecret,
            string correlationToken,
            string idToken)
        {
            _correlationToken = correlationToken;
            _tokenClient = new ClientCredentialsTokenRequest
            {
                Address = tokenUrl,
                ClientId = clientName,
                ClientSecret = clientSecret
            };
            _idToken = idToken;
        }

        public async Task<HttpClient> Create(Uri uri, string scope)
        {
            var client = new HttpClient() { BaseAddress = uri };
            _tokenClient.Scope = scope;
            // Запрашиваем маркер доступа у микросервиса Login
            var response = await client.RequestClientCredentialsTokenAsync(_tokenClient).ConfigureAwait(false);
            // Добавляем маркер доступа в исходящие запросы
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
            client.DefaultRequestHeaders.Add("Correlation-Token", _correlationToken);
            if (!string.IsNullOrEmpty(_idToken))
            {
                client.DefaultRequestHeaders.Add(Constants.END_USER_HEADER, _idToken);
            }
            return client;
        }
    }
}
