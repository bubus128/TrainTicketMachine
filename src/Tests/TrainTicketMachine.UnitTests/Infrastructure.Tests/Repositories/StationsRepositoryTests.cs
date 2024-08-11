using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using TrainTicketMachine.Infrastructure.Repositories;

namespace TrainTicketMachine.UnitTests.Infrastructure.Repositories
{
    public class StationsRepositoryTests
    {
        private Mock<IConfiguration> _configurationMock;
        private readonly Mock<HttpMessageHandler> _handlerMock = new(MockBehavior.Strict);
        private const string Url = "http://example.com/api";

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("InfrastructureConfig")["StationsApiUrl"]).Returns(Url);
        }

        /// <summary>
        /// Check if get request was send by checking if HttpMessageHandler.SendAsync was invoked.
        /// </summary>
        [Test]
        public void GetAllStations_Sends_Get_Request()
        {
            // Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                );
            var httpClient = new HttpClient(_handlerMock.Object);

            var stationsRepository = new StationsRepository(httpClient, _configurationMock.Object, new Mock<ILogger>().Object);

            // Act
            _ = stationsRepository.GetAllStations();

            // Assert
            _handlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.AtLeastOnce(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<System.Threading.CancellationToken>()
            );
        }

        /// <summary>
        /// Check if the method will return null when the request returns code 404.
        /// </summary>
        [Test]
        public async Task GetAllStations_Returns_Null_When_404()
        {
            // Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));
            var httpClient = new HttpClient(_handlerMock.Object);

            var stationsRepository = new StationsRepository(httpClient, _configurationMock.Object, new Mock<ILogger>().Object);

            // Act
            var stations = await stationsRepository.GetAllStations();

            // Assert
            Assert.IsNull(stations);
        }

        /// <summary>
        /// Check if the method will return null when the request returns code 500.
        /// </summary>
        [Test]
        public async Task GetAllStations_Returns_Null_When_500()
        {
            // Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var httpClient = new HttpClient(_handlerMock.Object);

            var stationsRepository = new StationsRepository(httpClient, _configurationMock.Object, new Mock<ILogger>().Object);

            // Act
            var stations = await stationsRepository.GetAllStations();

            // Assert
            Assert.IsNull(stations);
        }

        /// <summary>
        /// Check if the method returns null when an HttpRequestException occurs.
        /// </summary>
        [Test]
        public async Task GetAllStations_Returns_Null_When_HttpRequestException()
        {
            // Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Simulated HttpRequestException"));
            var httpClient = new HttpClient(_handlerMock.Object);

            var stationsRepository = new StationsRepository(httpClient, _configurationMock.Object, new Mock<ILogger>().Object);

            // Act
            var stations = await stationsRepository.GetAllStations();

            // Assert
            Assert.IsNull(stations);
        }

        /// <summary>
        /// Check if the method returns null when an JsonException occurs.
        /// </summary>
        [Test]
        public async Task GetAllStations_Returns_Null_When_JsonException()
        {
            // Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new JsonException("Simulated JsonException"));
            var httpClient = new HttpClient(_handlerMock.Object);

            var stationsRepository = new StationsRepository(httpClient, _configurationMock.Object, new Mock<ILogger>().Object);

            // Act
            var stations = await stationsRepository.GetAllStations();

            // Assert
            Assert.IsNull(stations);
        }
    }
}
