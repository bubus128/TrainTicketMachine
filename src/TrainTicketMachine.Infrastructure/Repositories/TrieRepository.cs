using System.Xml.Linq;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Repositories
{
    public class TrieRepository : ITrieRepository<TrieNode>
    {
        private List<TrieNode> _trieNodes;

        public TrieRepository()
        {
            _trieNodes = new List<TrieNode>();
        }

        /// <summary>
        /// Add station to a trie tee.
        /// </summary>
        /// <param name="station">Station to be aded into trie tree.</param>
        /// <exception cref="ArgumentNullException">Station argument cannot be null</exception>
        public void AddStation(Station station)
        {
            // Check if station is not null;
            if (station == null) throw new ArgumentNullException(nameof(station));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Prefix argument cannot be null.</exception>
        /// <exception cref="ArgumentException">Prefix argument cannot be empty</exception>
        public TrieNode GetNodeByPrefix(string prefix)
        {
            // Check if prefix is not null;
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));

            // Check if prefix is not empty
            if (prefix.Length == 0) throw new ArgumentException($"{nameof(prefix)} argument cannot be empty");

            throw new NotImplementedException();
        }
    }
}
