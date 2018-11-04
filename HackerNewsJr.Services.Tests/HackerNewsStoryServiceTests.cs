using AutoMapper;
using HackerNewsJr.App.Interfaces.Infrastructure.Http;
using HackerNewsJr.Services.Mappers;
using HackerNewsJr.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsJr.Services.Tests
{
    [TestClass]
    public class HackerNewsStoryServiceTests
    {
        private Mock<IJsonAPIClient> mockApiClient;
        private IMapper mockMapper;
        private HackerNewsStoryService service;

        [TestInitialize]
        public void Setup()
        {
            mockApiClient = new Mock<IJsonAPIClient>();
            mockMapper = new Mapper(
                new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            service = new HackerNewsStoryService(mockApiClient.Object, mockMapper);
        }

        [TestMethod]
        public async Task GetLatestStories_WithNonPositiveNumber_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() =>
                service.GetNewStoriesAsync(0));
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() =>
                service.GetNewStoriesAsync(-5));
        }

        [TestMethod]
        public async Task GetLatestStories_WhenNumberExceedsApiCapability_CallsApiForEachStoryUpToCapability()
        {
            // Arrange
            mockApiClient.Setup(c => c.GetAsync<int[]>(It.IsAny<string>()))
                .ReturnsAsync(new int[500]);

            // Act
            await service.GetNewStoriesAsync(600);

            // Assert
            mockApiClient.Verify(c => c.CachedGetAsync<Item>(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(500));
        }

        [TestMethod]
        public async Task GetLatestStories_WhenNumberIsLessThanApiCapability_CallsApiForEachStoryUpToNumber()
        {
            // Arrange
            mockApiClient.Setup(c => c.GetAsync<int[]>(It.IsAny<string>()))
                .ReturnsAsync(new int[500]);

            // Act
            await service.GetNewStoriesAsync(400);

            // Assert
            mockApiClient.Verify(c => c.CachedGetAsync<Item>(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(400));
        }

        [TestMethod]
        public async Task GetLatestStories_WithSomeNullResults_OnlyReturnsNonNullResults()
        {
            // Arrange
            mockApiClient.Setup(a => a.GetAsync<int[]>(It.IsAny<string>())).ReturnsAsync(new[] { 1, 2 });
            mockApiClient
                .Setup(a => a.CachedGetAsync<Item>(It.Is<string>(s => s.Contains("1")), It.IsAny<TimeSpan>()))
                .ReturnsAsync(new Item());
            mockApiClient
                .Setup(a => a.CachedGetAsync<Item>(It.Is<string>(s => s.Contains("2")), It.IsAny<TimeSpan>()))
                .ReturnsAsync((Item)null);

            // Act
            var results = await service.GetNewStoriesAsync(2);

            // Assert
            Assert.AreEqual(1, results.Count());
            Assert.IsNotNull(results.First());
        }
    }
}
