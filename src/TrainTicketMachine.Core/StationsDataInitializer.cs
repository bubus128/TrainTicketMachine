using TrainTicketMachine.Core.Interfaces;
using Microsoft.Extensions.Hosting;

public class StationsDataInitializer : IHostedService
{
    private readonly IStationService _stationService;

    public StationsDataInitializer(IStationService stationService)
    {
        _stationService = stationService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _stationService.FetchStationsData();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}