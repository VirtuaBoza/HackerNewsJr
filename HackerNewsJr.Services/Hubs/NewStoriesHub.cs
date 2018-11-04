using HackerNewsJr.App.Interfaces.Services.Hubs;
using HackerNewsJr.App.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.Services.Hubs
{
    public class NewStoriesHub : Hub<INewStoriesHub>
    {
        public async Task UpdateSubscribers(IEnumerable<Story> newStories)
        {
            await Clients.All.ReceiveNewStories(newStories);
        }
    }
}
