namespace TrainTicketMachine.Infrastructure.Models
{
    public class TrieRoot
    {
        public required List<TrieNode> TrieNodes { get; set; }

        public TrieRoot() 
        { 
            TrieNodes = new List<TrieNode>();
        }
    }
}
