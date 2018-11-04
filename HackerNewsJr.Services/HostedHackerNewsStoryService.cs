using HackerNewsJr.App.Interfaces.Services;
using HackerNewsJr.App.Interfaces.Services.Hubs;
using HackerNewsJr.App.Models;
using HackerNewsJr.Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNewsJr.Services
{
    public class HostedHackerNewsStoryService : IHostedService, IDisposable
    {
        private readonly ILogger<HostedHackerNewsStoryService> logger;
        private readonly IHubContext<NewStoriesHub, INewStoriesHub> newStoriesHubContext;
        private readonly IConfiguration config;
        private Timer timer;
        private IEnumerable<int> lastFetchedStoryIds;

        public HostedHackerNewsStoryService(
            ILogger<HostedHackerNewsStoryService> logger,
            IServiceProvider services,
            IHubContext<NewStoriesHub, INewStoriesHub> newStoriesHubContext,
            IConfiguration config)
        {
            this.logger = logger;
            this.newStoriesHubContext = newStoriesHubContext;
            this.config = config;
            Services = services;
            lastFetchedStoryIds = new List<int>();
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting timed HackerNewsStoryService.");
            var refreshTimeInSeconds = config.GetValue("StoryCacheRefreshInSeconds", 60);
            timer = new Timer(
                RefreshCache(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(refreshTimeInSeconds));
            return Task.CompletedTask;
        }

        private TimerCallback RefreshCache()
        {
            return async state =>
            {
                using (var scope = Services.CreateScope())
                {
                    var hackerNewsStoryService = scope.ServiceProvider
                        .GetRequiredService<IHackerNewsStoryService>();
                    var stories = (await hackerNewsStoryService.GetNewStoriesAsync(500)).ToList();
                    await UpdateSubscribers(stories);
                    lastFetchedStoryIds = stories.Select(s => s.Id);
                }
            };
        }

        private async Task UpdateSubscribers(IEnumerable<Story> stories)
        {
            var newStories = stories.Where(s => !lastFetchedStoryIds.Contains(s.Id)).ToList();
            if (newStories.Count > 0)
            {
                await newStoriesHubContext.Clients.All.ReceiveNewStories(newStories);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping timed HackerNewsStoryService.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
