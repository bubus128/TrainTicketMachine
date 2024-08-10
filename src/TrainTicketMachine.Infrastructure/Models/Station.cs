namespace TrainTicketMachine.Infrastructure.Models
{
    public class Station
    {
        public required string stationName { get; set; }
        public required string stationCode { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Station;

            return this.stationName == other.stationName
                && this.stationCode == other.stationCode;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
