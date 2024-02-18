using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Core
{
    public class StationService : IStationService
    {
        private IStationsRepository<Station> _stationsRepository;
        private ITrieRepository<TrieNode> _trieRepository;
        public StationService(IStationsRepository<Station> stationsRepository, ITrieRepository<TrieNode> trieRepository)
        {
            _stationsRepository = stationsRepository;
            _trieRepository = trieRepository;
        }
        public async Task FetchStationsData()
        {
            // Fetch stations
            List<Station>? stations = null;
            while(stations is null)
            {
                stations = await _stationsRepository.GetAllStations();
            }

            // Add stations into trie structure
            foreach (Station station in stations)
            {
                _trieRepository.AddStation(station);
            }
        }

        public async Task<SearchResponse?> GetStationsByPrefix(string prefix)
        {
            ArgumentException.ThrowIfNullOrEmpty(prefix, nameof(prefix));

            TrieNode? node = _trieRepository.GetNodeByPrefix(prefix);

            // Station not found
            if(node is null)
            {
                return new SearchResponse(){ StationsNames = new List<string>(), NextLetters = new List<char>()};
            }

            // Get the next possible letters
            List<char> nextLetters = node.Children.Keys.ToList();

            // Get all possible stations
            List<string> stationNames = new List<string>();
            fillResponse(node, stationNames);

            // Create new response 
            SearchResponse response = new SearchResponse()
            {
                NextLetters = nextLetters,
                StationsNames = stationNames
            };

            return response;
        }

        private void fillResponse(TrieNode node, List<string> stations)
        {
            if (node.Station is not null)
            {
                stations.Add(node.Station.stationName);
            }
            foreach(TrieNode nextNode in node.Children.Values)
            {
                fillResponse(nextNode, stations);
            }
        }
    }
}
