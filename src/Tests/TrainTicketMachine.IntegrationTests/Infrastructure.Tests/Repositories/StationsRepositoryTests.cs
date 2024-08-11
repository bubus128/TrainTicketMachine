using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TrainTicketMachine.Infrastructure.Repositories;

namespace TrainTicketMachine.FunctionalTests.Infrastructure.Tests.Repositories
{
    public class StationsRepositoryTests
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json").Build();
        }

        /// <summary>
        /// Check if method will get and parse stations data corectly.
        /// </summary>
        [Test]
        public async Task GetAllStations_Returns_List_Of_Stations()
        {
            // Arrange
            var stationsRepository = new StationsRepository(_httpClient, _configuration, new Mock<ILogger>().Object);

            // Act
            var stations = await stationsRepository.GetAllStations();

            // Assert
            Assert.That(stations, Is.Not.Null); // Check if not null
            Assert.That(stations.Count, Is.GreaterThan(0)); // Check if not empty
        }
    }
}
