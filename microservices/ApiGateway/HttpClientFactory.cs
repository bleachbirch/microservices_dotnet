using IdentityModel.Client;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ApiGateway
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

        public HttpClientFactory(string correlationToken)
        {
            _correlationToken = correlationToken;
            _tokenClient = new ClientCredentialsTokenRequest
            {
                Address = "http://localhost:5001/connect/token",
                ClientId = "api_gateway",
                ClientSecret = "secret"
            };
        }

        public HttpClientFactory(string idToken, string correlationToken) : this(correlationToken)
        {
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
                client.DefaultRequestHeaders.Add("pos-end-user", _idToken);
            }
            return client;
        }
    }
}
