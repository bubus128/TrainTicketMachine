using System.Runtime.CompilerServices;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Interfaces
{
    public interface IStationsRepository<T>
    {
        public Task<List<Station>> GetAllStation();
    }
}
