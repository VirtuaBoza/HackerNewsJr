using HackerNewsJr.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.App.Interfaces.Services.Hubs
{
    public interface INewStoriesHub
    {
        Task ReceiveNewStories(IEnumerable<Story> newStories);
    }
}
