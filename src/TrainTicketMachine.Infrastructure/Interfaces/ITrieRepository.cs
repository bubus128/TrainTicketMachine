using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Interfaces;

public interface ITrieRepository<TNode>
{
    public void AddStation(Station station);
    public TNode? GetNodeByPrefix(string prefix);
}
