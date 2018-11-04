using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsJr.Infrastructure.Http
{
    public class JsonAPIClient : IJsonAPIClient
    {
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;

        public JsonAPIClient(HttpClient httpClient, IMemoryCache memoryCache)
        {
            this.httpClient = httpClient;
            this.memoryCache = memoryCache;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var responseBody = await httpClient.GetStringAsync(endpoint);
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<T> CachedGetAsync<T>(string endpoint, TimeSpan slidingExpiration)
        {
            var responseBody = await memoryCache.GetOrCreateAsync(endpoint, async entry =>
            {
                entry.SlidingExpiration = slidingExpiration;
                return await httpClient.GetStringAsync(endpoint);
            });

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
