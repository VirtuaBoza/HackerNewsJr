using HackerNewsJr.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.App.Interfaces.Services
{
    public interface IHackerNewsStoryService
    {
        /// <summary>
        /// Returns the N newest stories with N specified by <paramref name="number"/> (limited by external API at 500).
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<IEnumerable<Story>> GetNewStoriesAsync(int number);
    }
}
