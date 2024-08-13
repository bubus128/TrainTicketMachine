using Microsoft.AspNetCore.Mvc.Testing;
namespace TrainTicketMachine.IntegrationTests.Api.Tests;

[TestFixture]
public class StationsControllerTests
{
    private HttpClient _client;

    [SetUp]
    public void SetUp()
    {
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Test]
    public async Task GetStationsByPrefix_WithEmptyPrefix_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/stations?prefix=");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetStationsByPrefix_WithValidPrefix_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/stations?prefix=abc");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }
}

