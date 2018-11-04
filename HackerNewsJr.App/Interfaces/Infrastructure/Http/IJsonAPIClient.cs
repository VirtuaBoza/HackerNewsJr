using System;
using System.Threading.Tasks;

namespace HackerNewsJr.App.Interfaces.Infrastructure.Http
{
    public interface IJsonAPIClient
    {
        /// <summary>
        /// Fetches data from JSON APIs and deserializes into type <typeparamref name="T"/> .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string endpoint);

        /// <summary>
        /// Fetches data from cache if available. Otherwise uses JSON API and deserializes into type <typeparamref name="T"/> .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        Task<T> CachedGetAsync<T>(string endpoint, TimeSpan slidingExpiration);
    }
}
