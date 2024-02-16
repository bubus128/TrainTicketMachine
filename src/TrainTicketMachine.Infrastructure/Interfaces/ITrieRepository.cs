using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Interfaces
{
    public interface ITrieRepository<Node>
    {
        public void AddStation(Station station);
        public Node GetNodeByPrefix(string prefix);
    }
}
