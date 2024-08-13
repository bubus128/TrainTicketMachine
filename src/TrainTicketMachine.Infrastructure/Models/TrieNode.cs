namespace TrainTicketMachine.Infrastructure.Models;

/// <summary>
///     Class representing the node of the trie tree.
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

        if (Children.Count != other.Children.Count)
        {
            return false;
        }

        foreach (var letter in Children.Keys)
        {
            if (!other.Children.ContainsKey(letter))
            {
                return false;
            }

            if (!Children[letter].Equals(other.Children[letter]))
            {
                return false;
            }
        }

        return Letter == other.Letter
               && Station == null
            ? other.Station == null
            : Station.Equals(other.Station);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
