using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Repositories
{
    public class StationsRepository : IStationsRepository<Station>
    {
        public StationsRepository(HttpClient httpClient, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Station>> GetAllStation()
        {
            throw new NotImplementedException();
        }
    }
}
