using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                TrieRepository repository = new TrieRepository();

                // Assert
                Assert.IsNotNull(repository._trieNodes);
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
                TrieRepository repository = new TrieRepository();

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
                TrieRepository repository = new TrieRepository();
                Station station = new Station()
                {
                    stationName = string.Empty,
                    stationCode = string.Empty,
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
                TrieRepository repository = new TrieRepository();
                string stationName = "Station";
                Station station = new Station()
                {
                    stationName = stationName,
                    stationCode = string.Empty,
                };

                // Act
                repository.AddStation(station);
                Dictionary<char, TrieNode> nodes = repository._trieNodes;

                // Assert
                Station? foundStation = null;
                foreach (char letter in stationName)
                {
                    Assert.True(nodes.ContainsKey(letter));
                    foundStation = nodes[letter].Station;
                    nodes = nodes[letter].Children;
                }
                Assert.AreEqual(foundStation, station);
                Assert.AreEqual(foundStation.stationName, stationName);
            }

            /// <summary>
            /// AddStation method should correctly add new station into existring node.
            /// </summary>
            [Test]
            public void Method_Adds_station_Correctly_In_Existring_Node()
            {
                // Arrange
                TrieRepository repository = new TrieRepository();
                string station1Name = "Station";
                string station2Name = "Statio";
                Station station1 = new Station()
                {
                    stationName = station1Name,
                    stationCode = string.Empty,
                };
                Station station2 = new Station()
                {
                    stationName = station2Name,
                    stationCode = string.Empty,
                };

                // Act
                repository.AddStation(station1);
                repository.AddStation(station2);
                Dictionary<char, TrieNode> nodes = repository._trieNodes;

                // Assert
                Station? foundStation = null;
                foreach (char letter in station2Name)
                {
                    Assert.True(nodes.ContainsKey(letter));
                    foundStation = nodes[letter].Station;
                    nodes = nodes[letter].Children;
                }
                Assert.AreEqual(foundStation, station2);
                Assert.AreEqual(foundStation.stationName, station2Name);
            }
        }

        public class GetNodeByPrefixTests
        {
            private TrieRepository _repository;

            [SetUp]
            public void SetUp()
            {
                // Create stations
                Station station1 = new Station() { stationCode = string.Empty, stationName = "s12" };
                Station station2 = new Station() { stationCode = string.Empty, stationName = "s23" };
                Station station3 = new Station() { stationCode = string.Empty, stationName = "a23" };

                // Create the trie structure
                TrieNode endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                TrieNode endNode2 = new TrieNode() { Letter = '3', Station = station2 };
                TrieNode endNode3 = new TrieNode() { Letter = '3', Station = station3 };

                TrieNode middleNode1 = new TrieNode() { Letter = '1'};
                middleNode1.Children.Add('2', endNode1 );
                TrieNode middleNode2 = new TrieNode() { Letter = '2' };
                middleNode2.Children.Add('3', endNode2);
                TrieNode middleNode3 = new TrieNode() { Letter = '3' };
                middleNode3.Children.Add('3', endNode3);

                TrieNode firstNode1 = new TrieNode() { Letter = 's' };
                firstNode1.Children.Add('1', middleNode1);
                firstNode1.Children.Add('2', middleNode2);
                TrieNode firstNode2 = new TrieNode() { Letter = 'a' };
                firstNode2.Children.Add('2', middleNode3);

                // Create the trie repository
                _repository = new TrieRepository();
                _repository._trieNodes.Add('s', firstNode1);
                _repository._trieNodes.Add('a', firstNode2);
            }

            /// <summary>
            /// GetNodeByPrefix method should throw ArgumentNullException when prefix argument is null.
            /// </summary>
            [Test]
            public void Method_Throws_ArgumentNullException_When_Prefix_Is_Null()
            {
                // Arrange
                TrieRepository repository = new TrieRepository();

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
                TrieRepository repository = new TrieRepository();

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
                TrieRepository repository = new TrieRepository();

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
                string prefix = "s12";
                Station station1 = new Station() { stationCode = string.Empty, stationName = "s12" };
                TrieNode endNode1 = new TrieNode() { Letter = '2', Station = station1 };

                // Act
                TrieNode node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.AreEqual(endNode1, node);
            }

            /// <summary>
            /// GetNodeByPrefix method should get correct middle node.
            /// </summary>
            [Test]
            public void Method_Gets_Coreect_Mid_Node()
            {
                // Arrange
                string prefix = "s1";
                Station station1 = new Station() { stationCode = string.Empty, stationName = "s12" };
                TrieNode endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                TrieNode middleNode1 = new TrieNode() { Letter = '1' };
                middleNode1.Children.Add('2', endNode1);

                // Act
                TrieNode node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.AreEqual(middleNode1, node);
            }

            /// <summary>
            /// GetNodeByPrefix method should get correct first node.
            /// </summary>
            [Test]
            public void Method_Gets_Coreect_First_Node()
            {
                // Arrange
                string prefix = "s";
                Station station1 = new Station() { stationCode = string.Empty, stationName = "s12" };
                Station station2 = new Station() { stationCode = string.Empty, stationName = "s23" };
                TrieNode endNode1 = new TrieNode() { Letter = '2', Station = station1 };
                TrieNode endNode2 = new TrieNode() { Letter = '3', Station = station2 };
                TrieNode middleNode1 = new TrieNode() { Letter = '1' };
                TrieNode middleNode2 = new TrieNode() { Letter = '2' };
                middleNode2.Children.Add('3', endNode2);
                middleNode1.Children.Add('2', endNode1);
                TrieNode firstNode1 = new TrieNode() { Letter = 's' };
                firstNode1.Children.Add('1', middleNode1);
                firstNode1.Children.Add('2', middleNode2);

                // Act
                TrieNode node = _repository.GetNodeByPrefix(prefix);

                // Assert
                Assert.AreEqual(firstNode1, node);
            }
        }
    }
}
