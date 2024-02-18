using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTicketMachine.Infrastructure.Models
{
    public class SearchResponse
    {
        public required List<string> StationsNames { get; set;  }
        public required List<char> NextLetters { get; set; }
    }
}
