using Microsoft.AspNetCore.Mvc;
using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationsController(IStationService stationService, ILogger logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<SearchResponse>> GetStationsByPrefix([FromQuery] string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return BadRequest("Prefix cannot be empty");
            }

            try
            {
                var searchResponse = await stationService.GetStationsByPrefix(prefix);

                if (searchResponse == null)
                {
                    return NotFound("No stations found for the given prefix");
                }

                return Ok(searchResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
