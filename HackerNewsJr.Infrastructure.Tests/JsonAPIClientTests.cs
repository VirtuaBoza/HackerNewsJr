using HackerNewsJr.Infrastructure.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNewsJr.Infrastructure.Tests
{
    [DataContract]
    public class TestObject
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [TestClass]
    public class JsonAPIClientTests
    {
        private Mock<HttpMessageHandler> mockHttpHandler;
        private Mock<IMemoryCache> mockMemCache;

        private JsonAPIClient client;

        private const string testApiEndpoint = "https://test.com/api/test";

        [TestInitialize]
        public void Setup()
        {
            mockHttpHandler = new Mock<HttpMessageHandler>();
            mockMemCache = new Mock<IMemoryCache>();

            var httpClient = new HttpClient(mockHttpHandler.Object);

            client = new JsonAPIClient(httpClient, mockMemCache.Object);
        }


        [TestMethod]
        public async Task GetAsync_WithNullResponse_ReturnsNull()
        {
            // Arrange
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null"),
                })
                .Verifiable();


            // Act
            var item = await client.GetAsync<TestObject>(testApiEndpoint);

            // Assert
            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task GetAsync_WithValidResponse_ReturnsHydratedType()
        {
            // Arrange
            var testId = 123;
            var testName = "Test Name";
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'id': " + testId + ",'name': '" + testName + "'}"),
                })
                .Verifiable();

            // Act
            var item = await client.GetAsync<TestObject>(testApiEndpoint);

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(testId, item.Id);
            Assert.AreEqual(testName, item.Name);
        }

        [TestMethod]
        public async Task CachedGetAsync_WithNullResponse_ReturnsNull()
        {
            // Arrange
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null"),
                })
                .Verifiable();
            mockMemCache.Setup(c => c.CreateEntry(testApiEndpoint))
                .Returns(new Mock<ICacheEntry>().Object);

            // Act
            var item = await client.CachedGetAsync<TestObject>(
                testApiEndpoint,
                TimeSpan.MinValue);

            // Assert
            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task CachedGetAsync_WithValidResponse_ReturnsHydratedType()
        {
            // Arrange
            var testId = 123;
            var testName = "Test Name";
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'id': " + testId + ",'name': '" + testName + "'}"),
                })
                .Verifiable();
            mockMemCache
                .Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);

            // Act
            var item = await client.CachedGetAsync<TestObject>(
                testApiEndpoint,
                TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(testId, item.Id);
            Assert.AreEqual(testName, item.Name);
        }

        [TestMethod]
        public async Task CachedGetAsync_WithoutCachedEntry_CallsAPI()
        {
            // Arrange
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'id': 1,'name': 'testName'}"),
                })
                .Verifiable();
            mockMemCache.Setup(c => c.CreateEntry(testApiEndpoint))
                .Returns(new Mock<ICacheEntry>().Object);

            // Act
            await client.CachedGetAsync<TestObject>(
                testApiEndpoint,
                TimeSpan.MinValue);

            // Assert
            mockHttpHandler.Verify();
        }

        [TestMethod]
        public async Task CachedGetAsync_WithCachedEntry_ReturnsCachedEntryInsteadOfCallingAPI()
        {
            // Arrange
            var testCachedResponse = (object)"{'id': 1,'name': 'testName'}";
            mockMemCache
                .Setup(c => c.TryGetValue(testApiEndpoint, out testCachedResponse))
                .Returns(true);

            // Act
            var result = await client.CachedGetAsync<TestObject>(
                testApiEndpoint,
                TimeSpan.MinValue);

            // Assert
            mockHttpHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Never(),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("testName", result.Name);
        }
    }
}
