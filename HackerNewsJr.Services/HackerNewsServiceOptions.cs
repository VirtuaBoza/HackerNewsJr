namespace HackerNewsJr.Services
{
    public class HackerNewsServiceOptions
    {
        public string HackerNewsApiUrl { get; set; }
        public int StoryCacheSlidingExpirationInDays { get; set; }
        public int StoryCacheRefreshInSeconds { get; set; }
    }
}
