using Microsoft.AspNetCore.Mvc;
using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationsController: ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse>> GetStationsByPrefix([FromQuery] string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return BadRequest("Prefix cannot be empty");
            }

            try
            {
                var searchResponse = await _stationService.GetStationsByPrefix(prefix);

                if (searchResponse == null)
                {
                    return NotFound("No stations found for the given prefix");
                }

                return Ok(searchResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
