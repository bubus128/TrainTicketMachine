using Moq;
using TrainTicketMachine.Core;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.UnitTests.Core.Tests;
public class StationServiceTests
{
    public class FetchStationsDataTests
    {
        private Mock<IStationsRepository<Station>> _stationsRepositoryMock;
        private Mock<ITrieRepository<TrieNode>> _trieRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _stationsRepositoryMock = new Mock<IStationsRepository<Station>>();
            _trieRepositoryMock = new Mock<ITrieRepository<TrieNode>>();
        }

        /// <summary>
        /// Method FetchStationsData should call StationsRepository.GetAllStations.
        /// </summary>
        [Test]
        public async Task Method_Calls_GetAllStations()
        {
            // Arrange
            _stationsRepositoryMock.Setup(x => x.GetAllStations()).ReturnsAsync([]);
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act
            await stationService.FetchStationsData();

            // Assert
            _stationsRepositoryMock.Verify(x => x.GetAllStations(), Times.Once());
        }

        /// <summary>
        /// Method FetchStationsData should call TrieRepository.AddStation for each station.
        /// </summary>
        [Test]
        public async Task Method_Calls_AddStation()
        {
            List<Station> stations =
            [
                new Station() { StationCode = string.Empty, StationName = string.Empty },
                    new Station() { StationCode = string.Empty, StationName = string.Empty },
                    new Station() { StationCode = string.Empty, StationName = string.Empty }
            ];
            _stationsRepositoryMock.Setup(x => x.GetAllStations()).ReturnsAsync(stations);

            // Arrange
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act
            await stationService.FetchStationsData();

            // Assert
            _trieRepositoryMock.Verify(x => x.AddStation(It.IsAny<Station>()), Times.Exactly(stations.Count));
        }

        /// <summary>
        /// Method FetchStationsData should recall StationsRepository.GetAllStations when the first call returns null.
        /// </summary>
        [Test]
        public async Task Method_Retries_To_GetAllStations()
        {
            // Arrange
            _stationsRepositoryMock.SetupSequence(x => x.GetAllStations())
                .ReturnsAsync((List<Station>)null)
                .ReturnsAsync([]);
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act
            await stationService.FetchStationsData();

            // Assert
            _stationsRepositoryMock.Verify(x => x.GetAllStations(), Times.Exactly(2));
        }
    }


    public class GetStationsByPrefixTests()
    {
        private Mock<IStationsRepository<Station>> _stationsRepositoryMock;
        private Mock<ITrieRepository<TrieNode>> _trieRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _stationsRepositoryMock = new Mock<IStationsRepository<Station>>();
            _trieRepositoryMock = new Mock<ITrieRepository<TrieNode>>();
        }

        /// <summary>
        /// GetStationsByPrefix method should throw the ArgumentException when prefix argument is null;
        /// </summary>
        [Test]
        public void Method_Throws_ArgumentNullException_When_Argument_Null()
        {
            // Arrange
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => stationService.GetStationsByPrefix(null));
        }

        /// <summary>
        /// GetStationsByPrefix method should throw the ArgumentException when prefix argument is empty;
        /// </summary>
        [Test]
        public void Method_Throws_ArgumentException_When_Argument_Empty()
        {
            // Arrange
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => stationService.GetStationsByPrefix(string.Empty));
        }

        /// <summary>
        /// GetStationsByPrefix method should call TrieRepository.GetNodeByPrefix.
        /// </summary>
        [Test]
        public void Method_Calls_GetAllStations()
        {
            // Arrange
            const string prefix = "abc";
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act
            _ = stationService.GetStationsByPrefix(prefix);

            // Assert
            _trieRepositoryMock.Verify(x => x.GetNodeByPrefix(prefix), Times.Once());
        }

        /// <summary>
        /// GetStationsByPrefix should return correct SearchResponse object.
        /// </summary>
        [Test]
        public async Task Method_Returns_Correct_ResponseAsync()
        {
            // Arrange
            const string prefix = "abc";
            const string stationName = "abc";
            var station = new Station() { StationCode = string.Empty, StationName = stationName };
            var node = new TrieNode() { Letter = 'c', Station = station };
            _trieRepositoryMock.Setup(x => x.GetNodeByPrefix(It.IsAny<string>())).Returns(node);
            var stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

            // Act
            var response = await stationService.GetStationsByPrefix(prefix);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StationsNames, Has.Count.EqualTo(1));
            Assert.That(response.StationsNames[0], Is.EqualTo(stationName));
            Assert.That(response.NextLetters, Is.Empty);
        }
    }
}
