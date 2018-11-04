using AutoMapper;
using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using HackerNewsJr.App.Interfaces.Services;
using HackerNewsJr.App.Models;
using HackerNewsJr.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.Services
{
    public class HackerNewsStoryService : IHackerNewsStoryService
    {
        private const string hackerNewsApiUrl =
            "https://hacker-news.firebaseio.com/v0";

        private readonly IJsonAPIClient apiClient;
        private readonly IMapper mapper;
        private readonly ILogger<HackerNewsStoryService> logger;
        private readonly IConfiguration config;


        public HackerNewsStoryService(
            IJsonAPIClient apiClient,
            IMapper mapper,
            ILogger<HackerNewsStoryService> logger,
            IConfiguration config)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
            this.logger = logger;
            this.config = config;
        }

        public async Task<IEnumerable<Story>> GetNewStoriesAsync(int numberToRetrieve)
        {
            if (numberToRetrieve < 1) throw new ArgumentOutOfRangeException();

            logger.LogInformation("Fetching latest stories.");
            var newStoryIds = await apiClient.GetAsync<int[]>($"{hackerNewsApiUrl}/newstories.json");

            numberToRetrieve = numberToRetrieve < newStoryIds.Length
                ? numberToRetrieve
                : newStoryIds.Length;

            var items = new List<Item>();

            for (var i = 0; i < numberToRetrieve; i++)
            {
                var storyId = newStoryIds[i];
                var item =
                    await apiClient.CachedGetAsync<Item>(
                        $"{hackerNewsApiUrl}/item/{storyId}.json",
                        TimeSpan.FromDays(config.GetValue("StoryCacheSlidingExpiryInDays", 3)));

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return mapper.Map<IEnumerable<Item>, IEnumerable<Story>>(items);
        }
    }
}
