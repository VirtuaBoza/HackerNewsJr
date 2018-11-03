using AutoMapper;
using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using HackerNewsJr.App.Interfaces.Services;
using HackerNewsJr.App.Models;
using HackerNewsJr.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.Services
{
    public class HackerNewsStoryService : IHackerNewsStoryService
    {
        private readonly IJsonAPIClient apiClient;
        private readonly IMapper mapper;

        private const string hackerNewsApiUrl =
            "https://hacker-news.firebaseio.com/v0";

        public HackerNewsStoryService(IJsonAPIClient apiClient, IMapper mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Story>> GetNewStoriesAsync(int numberToRetrieve)
        {
            if (numberToRetrieve < 1) throw new ArgumentOutOfRangeException();

            var newStoryIds = await apiClient.GetAsync<int[]>($"{hackerNewsApiUrl}/newstories.json");

            numberToRetrieve = numberToRetrieve < newStoryIds.Length
                ? numberToRetrieve
                : newStoryIds.Length;

            var items = new List<Item>();

            for (var i = 0; i < numberToRetrieve; i++)
            {
                var storyId = newStoryIds[i];
                var item =
                    await apiClient.GetAsync<Item>(
                        $"{hackerNewsApiUrl}/item/{storyId}.json");

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return mapper.Map<IEnumerable<Item>, IEnumerable<Story>>(items);
        }
    }
}
