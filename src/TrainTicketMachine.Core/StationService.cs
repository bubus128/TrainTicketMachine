using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Core;

public class StationService(
    IStationsRepository<Station> stationsRepository,
    ITrieRepository<TrieNode> trieRepository)
    : IStationService
{
    public async Task FetchStationsData()
    {
        // Fetch stations
        List<Station>? stations = null;
        while (stations is null)
        {
            stations = await stationsRepository.GetAllStations();
        }

        // Add stations into trie structure
        foreach (var station in stations)
        {
            trieRepository.AddStation(station);
        }
    }

    public async Task<SearchResponse?> GetStationsByPrefix(string prefix)
    {
        ArgumentException.ThrowIfNullOrEmpty(prefix, nameof(prefix));

        var node = trieRepository.GetNodeByPrefix(prefix);

        // Station not found
        if (node is null)
        {
            return new SearchResponse { StationsNames = [], NextLetters = [] };
        }

        // Get the next possible letters
        var nextLetters = node.Children.Keys.ToList();

        // Get all possible stations
        var stationNames = new List<string>();
        FillResponse(node, stationNames);

        // Create new response 
        var response = new SearchResponse { NextLetters = nextLetters, StationsNames = stationNames };

        return response;
    }

    private void FillResponse(TrieNode node, List<string> stations)
    {
        if (node.Station is not null)
        {
            stations.Add(node.Station.StationName);
        }

        foreach (var nextNode in node.Children.Values)
        {
            FillResponse(nextNode, stations);
        }
    }
}
