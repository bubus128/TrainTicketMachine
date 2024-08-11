using Microsoft.Extensions.Hosting;
using TrainTicketMachine.Core.Interfaces;

namespace TrainTicketMachine.Core;

public class StationsDataInitializer(IStationService stationService) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await stationService.FetchStationsData();

            // Wait for the next interval
            await Task.Delay(_interval, stoppingToken);
        }
    }
}