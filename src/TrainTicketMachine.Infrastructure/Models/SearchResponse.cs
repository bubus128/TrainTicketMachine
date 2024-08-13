namespace TrainTicketMachine.Infrastructure.Models;

public class SearchResponse
{
    public required List<string> StationsNames { get; set; }
    public required List<char> NextLetters { get; set; }
}
