namespace TrainTicketMachine.Infrastructure.Models
{
    /// <summary>
    /// Class representing the node of the trie tree.
    /// </summary>
    public class TrieNode
    {
        public required char Char {  get; set; }
        public required List<TrieNode> Children {  get; set; }
        public Station? Station { get; set; }

        public TrieNode() 
        { 
            // Init the list of children
            Children = new List<TrieNode>();
        }
    }
}
