using AutoMapper;
using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using HackerNewsJr.App.Interfaces.Services;
using HackerNewsJr.App.Models;
using HackerNewsJr.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsJr.Services
{
    public class HackerNewsStoryService : IHackerNewsStoryService
    {
        private readonly IJsonAPIClient apiClient;
        private readonly IMapper mapper;
        private readonly ILogger<HackerNewsStoryService> logger;

        public HackerNewsStoryService(
            IJsonAPIClient apiClient,
            IMapper mapper,
            ILogger<HackerNewsStoryService> logger,
            IOptions<HackerNewsServiceOptions> optionsAccessor)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
            this.logger = logger;
            Options = optionsAccessor.Value;
        }

        public HackerNewsServiceOptions Options { get; }

        public async Task<IEnumerable<Story>> GetNewStoriesAsync(int numberToRetrieve)
        {
            if (numberToRetrieve < 1) throw new ArgumentOutOfRangeException();

            logger.LogInformation("Fetching latest stories.");
            var newStoryIds = await apiClient
                .GetAsync<int[]>($"{Options.HackerNewsApiUrl}/newstories.json");

            numberToRetrieve = numberToRetrieve < newStoryIds.Length
                ? numberToRetrieve
                : newStoryIds.Length;

            var items = await RetrieveFirstItems(numberToRetrieve, newStoryIds);

            return mapper.Map<IEnumerable<Item>, IEnumerable<Story>>(items);
        }

        private async Task<IEnumerable<Item>> RetrieveFirstItems(
            int numberToRetrieve,
            IReadOnlyList<int> itemIds)
        {
            var items = new List<Item>();

            for (var i = 0; i < numberToRetrieve; i++)
            {
                var itemId = itemIds[i];
                var item =
                    await apiClient.CachedGetAsync<Item>(
                        $"{Options.HackerNewsApiUrl}/item/{itemId}.json",
                        TimeSpan.FromDays(Options.StoryCacheSlidingExpirationInDays));

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
