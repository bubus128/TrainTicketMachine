using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTicketMachine.Infrastructure.Repositories;
using System.IO;
using TrainTicketMachine.Infrastructure.Models;

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
        /// <returns></returns>
        [Test]
        public async Task GetAllStations_Returns_List_Of_Stations()
        {
            // Arrange
            StationsRepository stationsRepository = new StationsRepository(_httpClient, _configuration);

            // Act
            List<Station>? stations= await stationsRepository.GetAllStation();

            // Assert
            Assert.IsNotNull(stations); // Check if not null
            Assert.Greater(stations.Count, 0); // Check if not empty
        }
    }
}
