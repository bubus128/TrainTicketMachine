using Moq;
using TrainTicketMachine.Core;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.UnitTests.Core.Tests
{
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
                _stationsRepositoryMock.Setup(x => x.GetAllStations()).ReturnsAsync(new List<Station>());
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

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
                List<Station> stations = new List<Station>()
            {
                new Station(){ stationCode = string.Empty, stationName = string.Empty },
                new Station(){ stationCode = string.Empty, stationName = string.Empty },
                new Station(){ stationCode = string.Empty, stationName = string.Empty },
            };
                _stationsRepositoryMock.Setup(x => x.GetAllStations()).ReturnsAsync(stations);

                // Arrange
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

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
                    .ReturnsAsync(new List<Station>());
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

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
            public void Method_Throws_ArgumentNullException_When_Arguemnt_Null()
            {
                // Arrange
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => stationService.GetStationsByPrefix(null));
            }

            /// <summary>
            /// GetStationsByPrefix method should throw the ArgumentException when prefix argument is empty;
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentException_When_Arguemnt_Empty()
            {
                // Arrange
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => stationService.GetStationsByPrefix(string.Empty));
            }

            /// <summary>
            /// GetStationsByPrefix method should call TrieRepository.GetNodeByPrefix.
            /// </summary>
            [Test]
            public void Method_Calls_GetAllStations()
            {
                // Arrange
                string prefix = "abc";
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

                // Act
                stationService.GetStationsByPrefix(prefix);

                // Assert
                _trieRepositoryMock.Verify(x => x.GetNodeByPrefix(prefix), Times.Once());
            }

            /// <summary>
            /// GetStationsByPrefix should return correct SearchResponse object.
            /// </summary>
            [Test]
            public void Method_Returns_Correct_Response()
            {
                // Arrange
                string prefix = "abc";
                string stationName = "abc";
                Station station = new Station() { stationCode = string.Empty, stationName = stationName };
                TrieNode node = new TrieNode() { Letter = 'c', Station = station };
                _trieRepositoryMock.Setup(x => x.GetNodeByPrefix(It.IsAny<string>())).Returns(node);
                StationService stationService = new StationService(_stationsRepositoryMock.Object, _trieRepositoryMock.Object);

                // Act
                SearchResponse response = stationService.GetStationsByPrefix(prefix);

                // Assert
                Assert.IsNotNull(response);
                Assert.AreEqual(response.StationsNames.Count, 1);
                Assert.AreEqual(response.StationsNames[0], stationName);
                Assert.AreEqual(response.NextLetters.Count, 0);
            }
        }
    }
}
