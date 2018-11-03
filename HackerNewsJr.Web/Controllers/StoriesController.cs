using HackerNewsJr.App.Interfaces.Services;
using HackerNewsJr.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.Web.Controllers
{
    [Route("api/[controller]")]
    public class StoriesController : Controller
    {
        private readonly IHackerNewsStoryService storyService;

        public StoriesController(IHackerNewsStoryService storyService)
        {
            this.storyService = storyService;
        }

        [HttpGet("[action]/{number}")]
        public async Task<IEnumerable<Story>> NewStories(int number)
        {
            return await storyService.GetNewStoriesAsync(number);
        }
    }
}
