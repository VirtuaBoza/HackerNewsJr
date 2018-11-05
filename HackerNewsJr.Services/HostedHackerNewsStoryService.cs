using HackerNewsJr.App.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNewsJr.Services
{
    public class HostedHackerNewsStoryService : IHostedService, IDisposable
    {
        private readonly ILogger<HostedHackerNewsStoryService> logger;
        private Timer timer;

        public HostedHackerNewsStoryService(
            ILogger<HostedHackerNewsStoryService> logger,
            IServiceProvider services,
            IOptions<HackerNewsServiceOptions> optionsAccessor)
        {
            this.logger = logger;
            Services = services;
            Options = optionsAccessor.Value;
        }

        public IServiceProvider Services { get; }
        public HackerNewsServiceOptions Options { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting timed HackerNewsStoryService.");

            timer = new Timer(
                RefreshCache(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(Options.StoryCacheRefreshInSeconds));

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
                    await hackerNewsStoryService.GetNewStoriesAsync(500);
                }
            };
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
