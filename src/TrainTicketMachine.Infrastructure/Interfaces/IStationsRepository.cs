using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Interfaces
{
    public interface IStationsRepository<T>
    {
        public Task<List<Station>?> GetAllStations();
    }
}
