using HackerNewsJr.App.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            IServiceProvider services)
        {
            this.logger = logger;
            Services = services;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting timed HackerNewsStoryService.");

            timer = new Timer(async state =>
            {
                using (var scope = Services.CreateScope())
                {
                    var hackerNewsStoryService = scope.ServiceProvider
                        .GetRequiredService<IHackerNewsStoryService>();
                    await hackerNewsStoryService.GetNewStoriesAsync(500);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
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
