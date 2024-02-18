using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
using TrainTicketMachine.Infrastructure.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.Infrastructure.Repositories
{
    public class StationsRepository : IStationsRepository<Station>
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public StationsRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration.GetSection("InfrastructureConfig")["StationsApiUrl"];
        }

        public async Task<List<Station>?> GetAllStations()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    List<Station>? stations = JsonSerializer.Deserialize<List<Station>>(jsonString);

                    return stations;
                }
                else
                {
                    // Handle unsuccessful response
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Handle 404 Not Found
                        Console.WriteLine("The requested resource was not found.");
                    }
                    else
                    {
                        // Handle other types of errors
                        Console.WriteLine($"Failed to retrieve data from API. Status code: {response.StatusCode}");
                    }
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network errors
                Console.WriteLine($"An error occurred while sending the request: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                Console.WriteLine($"An error occurred while deserializing JSON response: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle other types of errors
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
