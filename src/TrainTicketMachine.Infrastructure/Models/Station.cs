namespace TrainTicketMachine.Infrastructure.Models;

public class Station
{
    public required string StationName { get; set; }
    public required string StationCode { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = obj as Station;

        return StationName == other.StationName
               && StationCode == other.StationCode;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
