using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Repositories
{
    public class TrieRepository : ITrieRepository<TrieNode>
    {
        public Dictionary<char, TrieNode> TrieNodes { get; } = [];

        /// <summary>
        /// Add station to a trie tee.
        /// </summary>
        /// <param name="station">Station to be added into trie tree.</param>
        /// <exception cref="ArgumentNullException">Station argument cannot be null</exception>
        public void AddStation(Station station)
        {
            // Check if station is not null;
            ArgumentNullException.ThrowIfNull(station);

            if (station.StationName.Length == 0)
            {
                throw new ArgumentException("Station name cannot be empty");
            }

            TrieNode? currentNode = null;
            var children = TrieNodes;
            var upperName = station.StationName.ToUpper();
            foreach (var letter in upperName)
            {
                if (!children.ContainsKey(letter))
                {
                    children.Add(letter, new TrieNode() { Letter = letter });
                }

                currentNode = children[letter];
                children = currentNode.Children;
            }
            currentNode.Station = station;
        }

        /// <summary>
        /// Get the trie node that represents the prefix.
        /// </summary>
        /// <param name="prefix">Prefix of the station name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Prefix argument cannot be null.</exception>
        /// <exception cref="ArgumentException">Prefix argument cannot be empty</exception>
        public TrieNode? GetNodeByPrefix(string prefix)
        {
            // Check if prefix is not null;
            ArgumentNullException.ThrowIfNull(prefix);

            prefix = prefix.ToUpper();

            // Check if prefix is not empty
            if (prefix.Length == 0) throw new ArgumentException($"{nameof(prefix)} argument cannot be empty");

            TrieNode? currentNode = null;
            var children = TrieNodes;
            foreach (var letter in prefix)
            {
                if (!children.ContainsKey(letter))
                {
                    return null;
                }

                currentNode = children[letter];
                children = currentNode.Children;
            }

            return currentNode;
        }
    }
}
