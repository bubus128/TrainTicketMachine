namespace TrainTicketMachine.Infrastructure.Models
{
    /// <summary>
    /// Class representing the node of the trie tree.
    /// </summary>
    public class TrieNode
    {
        public required char Letter { get; set; }
        public Dictionary<char, TrieNode> Children { get; set; } = new();
        public Station? Station { get; set; }

        // Init the list of children

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as TrieNode;

            if (this.Children.Count != other.Children.Count)
            {
                return false;
            }

            foreach (var letter in this.Children.Keys)
            {
                if (!other.Children.ContainsKey(letter))
                {
                    return false;
                }
                if (!this.Children[letter].Equals(other.Children[letter]))
                {
                    return false;
                }
            }

            return this.Letter == other.Letter
                && this.Station == null ? other.Station == null : this.Station.Equals(other.Station);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
