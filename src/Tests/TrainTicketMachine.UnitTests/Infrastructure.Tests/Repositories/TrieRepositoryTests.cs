using TrainTicketMachine.Infrastructure.Models;
using TrainTicketMachine.Infrastructure.Repositories;

namespace TrainTicketMachine.UnitTests.Infrastructure.Tests.Repositories
{
    public class TrieRepositoryTests
    {
        public class ConstructorTests
        {
            /// <summary>
            /// Constructior should initialize the node list field;
            /// </summary>
            [Test]
            public void Constructor_Inits_Nodes_List()
            {
                // Arrange

                // Act
                var repository = new TrieRepository();

                // Assert
                Assert.IsNotNull(repository.TrieNodes);
            }
        }

        public class AddStationTests
        {
            /// <summary>
            /// AddStation method should throw ArgumentNullException when station argument is null.
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentNullException_When_Station_Is_Null()
            {
                // Arrange
                var repository = new TrieRepository();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => repository.AddStation(null));
            }

            /// <summary>
            /// AddStation method should throw ArgumentException when station argument has empty stationName field.
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentException_When_Station_Name_Is_Empty()
            {
                // Arrange
                var repository = new TrieRepository();
                var station = new Station()
                {
                    StationName = string.Empty,
                    StationCode = string.Empty,
                };

                // Act & Assert
                Assert.Throws<ArgumentException>(() => repository.AddStation(station));
            }

            /// <summary>
            /// AddStation method should correctly add new station into new node.
            /// </summary>
            [Test]
            public void Method_Adds_station_Correctly_In_New_Node()
            {
                // Arrange
                var repository = new TrieRepository();
                const string stationName = "Station";
                var station = new Station()
                {
                    StationName = stationName,
                    StationCode = string.Empty,
                };

                // Act
                repository.AddStation(station);
                var nodes = repository.TrieNodes;

                // Assert
                Station? foundStation = null;
                foreach (var letter in stationName.ToUpper())
                {
                    Assert.True(nodes.ContainsKey(letter));
                    foundStation = nodes[letter].Station;
                    nodes = nodes[letter].Children;
                }
                Assert.That(station, Is.EqualTo(foundStation));
                Assert.That(foundStation.StationName, Is.EqualTo(stationName));
            }

            /// <summary>
            /// AddStation method should correctly add new station into existring node.
            /// </summary>
            [Test]
            public void Method_Adds_station_Correctly_In_Existring_Node()
            {
                // Arrange
                var repository = new TrieRepository();
                var station1Name = "Station";
                var station2Name = "Statio";
                var station1 = new Station()
                {
                    StationName = station1Name,
                    StationCode = string.Empty,
                };
                var station2 = new Station()
                {
                    StationName = station2Name,
                    StationCode = string.Empty,
                };

                // Act
                repository.AddStation(station1);
                repository.AddStation(station2);
                var nodes = repository.TrieNodes;

                // Assert
                Station? foundStation = null;
                foreach (var letter in station2Name.ToUpper())
                {
                    Assert.True(nodes.ContainsKey(letter));
                    foundStation = nodes[letter].Station;
                    nodes = nodes[letter].Children;
                }
                Assert.That(station2, Is.EqualTo(foundStation));
                Assert.That(station2Name, Is.EqualTo(foundStation.StationName));
            }
        }

        public class GetNodeByPrefixTests
        {
            private TrieRepository _repository;

            [SetUp]
            public void SetUp()
            {
                // Create stations
                var station1 = new Station() { StationCode = string.Empty, StationName = "s12" };
                var station2 = new Station() { StationCode = string.Empty, StationName = "s23" };
                var station3 = new Station() { StationCode = string.Empty, StationName = "a23" };

                // Create the trie structure
                var endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                var endNode2 = new TrieNode() { Letter = '3', Station = station2 };
                var endNode3 = new TrieNode() { Letter = '3', Station = station3 };

                var middleNode1 = new TrieNode() { Letter = '1' };
                middleNode1.Children.Add('2', endNode1);
                var middleNode2 = new TrieNode() { Letter = '2' };
                middleNode2.Children.Add('3', endNode2);
                var middleNode3 = new TrieNode() { Letter = '3' };
                middleNode3.Children.Add('3', endNode3);

                var firstNode1 = new TrieNode() { Letter = 'S' };
                firstNode1.Children.Add('1', middleNode1);
                firstNode1.Children.Add('2', middleNode2);
                var firstNode2 = new TrieNode() { Letter = 'A' };
                firstNode2.Children.Add('2', middleNode3);

                // Create the trie repository
                _repository = new TrieRepository();
                _repository.TrieNodes.Add('S', firstNode1);
                _repository.TrieNodes.Add('A', firstNode2);
            }

            /// <summary>
            /// GetNodeByPrefix method should throw ArgumentNullException when prefix argument is null.
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentNullException_When_Prefix_Is_Null()
            {
                // Arrange
                var repository = new TrieRepository();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => repository.GetNodeByPrefix(null));
            }

            /// <summary>
            /// GetNodeByPrefix method should throw ArgumentException when prefix argument is empty.
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentException_When_Prefix_Is_Empty()
            {
                // Arrange
                var repository = new TrieRepository();

                // Act & Assert
                Assert.Throws<ArgumentException>(() => repository.GetNodeByPrefix(string.Empty));
            }

            /// <summary>
            /// GetNodeByPrefix method should return null when tree node does not exist for given prefix.
            /// </summary>
            [Test]
            public void Method_Returns_Null_When_Node_Not_Found()
            {
                // Arrange
                var repository = new TrieRepository();

                // Act & Assert
                Assert.Throws<ArgumentException>(() => repository.GetNodeByPrefix(string.Empty));
            }

            /// <summary>
            /// GetNodeByPrefix method should get correct end node.
            /// </summary>
            [Test]
            public void Method_Gets_Coreect_End_Node()
            {
                // Arrange
                var prefix = "s12";
                var station1 = new Station() { StationCode = string.Empty, StationName = "s12" };
                var endNode1 = new TrieNode() { Letter = '2', Station = station1 };

                // Act
                var node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.That(node, Is.EqualTo(endNode1));
            }

            /// <summary>
            /// GetNodeByPrefix method should get correct middle node.
            /// </summary>
            [Test]
            public void Method_Gets_Coreect_Mid_Node()
            {
                // Arrange
                var prefix = "s1";
                var station1 = new Station() { StationCode = string.Empty, StationName = "s12" };
                var endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                var middleNode1 = new TrieNode() { Letter = '1' };
                middleNode1.Children.Add('2', endNode1);

                // Act
                var node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.That(node, Is.EqualTo(middleNode1));
            }

            /// <summary>
            /// GetNodeByPrefix method should get correct first node.
            /// </summary>
            [Test]
            public void Method_Gets_Coreect_First_Node()
            {
                // Arrange
                var prefix = "s";
                var station1 = new Station() { StationCode = string.Empty, StationName = "s12" };
                var station2 = new Station() { StationCode = string.Empty, StationName = "s23" };
                var endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                var endNode2 = new TrieNode() { Letter = '3', Station = station2 };
                var middleNode1 = new TrieNode() { Letter = '1' };
                var middleNode2 = new TrieNode() { Letter = '2' };
                middleNode2.Children.Add('3', endNode2);
                middleNode1.Children.Add('2', endNode1);
                var firstNode1 = new TrieNode() { Letter = 'S' };
                firstNode1.Children.Add('1', middleNode1);
                firstNode1.Children.Add('2', middleNode2);

                // Act
                var node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.That(node, Is.EqualTo(firstNode1));
            }
        }
    }
}
