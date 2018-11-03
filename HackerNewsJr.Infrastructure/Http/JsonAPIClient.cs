using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsJr.Infrastructure.Http
{
    public class JsonAPIClient : IJsonAPIClient
    {
        private readonly HttpClient httpClient;

        public JsonAPIClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var responseBody = await httpClient.GetStringAsync(endpoint);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
