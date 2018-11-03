using HackerNewsJr.Infrastructure.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
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
        [TestMethod]
        public async Task GetAsync_WithNullResponse_ReturnsNull()
        {
            // Arrange
            var mockHttpHandler = new Mock<HttpMessageHandler>();
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

            var httpClient = new HttpClient(mockHttpHandler.Object);
            var client = new JsonAPIClient(httpClient);

            // Act
            var item = await client.GetAsync<TestObject>("https://test.com/api/test");

            // Assert
            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task GetAsync_WithValidResponse_ReturnsHydratedType()
        {
            // Arrange
            var testId = 123;
            var testName = "Test Name";
            var mockHttpHandler = new Mock<HttpMessageHandler>();
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

            var httpClient = new HttpClient(mockHttpHandler.Object);
            var client = new JsonAPIClient(httpClient);

            // Act
            var item = await client.GetAsync<TestObject>("https://test.com/api/test");

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(testId, item.Id);
            Assert.AreEqual(testName, item.Name);
        }
    }
}
