using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTicketMachine.Infrastructure.Models;
using static System.Collections.Specialized.BitVector32;

namespace TrainTicketMachine.Core.Interfaces
{
    public interface IStationService
    {
        public Task<SearchResponse?> GetStationsByPrefix(string prefix);
        public Task FetchStationsData();
    }
}
