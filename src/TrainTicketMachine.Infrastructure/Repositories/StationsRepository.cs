using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Repositories
{
    public class StationsRepository(HttpClient httpClient, IConfiguration configuration, ILogger<StationsRepository> logger) : IStationsRepository<Station>
    {
        private readonly string? _apiUrl = configuration.GetSection("InfrastructureConfig")["StationsApiUrl"];

        public async Task<List<Station>?> GetAllStations()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            try
            {
                var response = await httpClient.GetAsync(_apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var stations = JsonSerializer.Deserialize<List<Station>>(jsonString, options);

                    return stations;
                }

                // Handle 404 Not Found
                logger.LogError(response.StatusCode == HttpStatusCode.NotFound
                    ? "The requested resource was not found."
                    // Handle other types of errors
                    : $"Failed to retrieve data from API. Status code: {response.StatusCode}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                // Handle network errors
                logger.LogError(ex, $"An error occurred while sending the request: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                logger.LogError(ex, $"An error occurred while deserializing JSON response: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle other types of errors
                logger.LogError(ex, $"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
