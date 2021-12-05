using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Text;

namespace ApiGateway
{
    public class LoyaltyProgramClient
    {
        //Стратегия повтора запросов
        private static AsyncRetryPolicy _exponentialRetryPolicy =
            Policy.Handle<Exception>().WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));
        private readonly string _hostName = "http://localhost:5126/";

        public async Task<HttpResponseMessage> RegisterUser(LoyaltyProgramUser newUser)
        {
            return await _exponentialRetryPolicy.ExecuteAsync(() => DoRegisterUser(newUser));
        }

        private async Task<HttpResponseMessage> DoRegisterUser(LoyaltyProgramUser newUser)
        {
            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_hostName);
                var response = await httpClient.PostAsync("/users/",
                    new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json"));
                ThtowOnTransientFailure(response);
                return response;
            }
        }

        /// <summary>
        /// Генерирует исключение для указания стратегрии на необходимость повтора
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="Exception"></exception>
        private void ThtowOnTransientFailure(HttpResponseMessage response)
        {
            if((int)response.StatusCode < 200 || (int)response.StatusCode > 499)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }

}
