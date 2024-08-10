using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Core.Interfaces;

public interface IStationService
{
    public Task<SearchResponse?> GetStationsByPrefix(string prefix);
    public Task FetchStationsData();
}
