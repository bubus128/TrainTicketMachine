using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrainTicketMachine.Api.Controllers;
using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.UnitTests.Api.Tests.Controllers;
public class StationsControllerTests
{
    private Mock<IStationService> _mockStationService;
    private StationsController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockStationService = new Mock<IStationService>();
        _controller = new StationsController(_mockStationService.Object, new Mock<ILogger<StationsController>>().Object);
    }

    /// <summary>
    /// Method should return ok result.
    /// </summary>
    [Test]
    public async Task GetStationsByPrefix_WithValidPrefix_ReturnsOkResult()
    {
        // Arrange
        const string prefix = "abc";
        var expectedResponse = new SearchResponse
        {
            NextLetters = ['d', 'e', 'f'],
            StationsNames = ["Station1", "Station2"]
        };
        _mockStationService.Setup(x => x.GetStationsByPrefix(prefix)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetStationsByPrefix(prefix);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
    }

    /// <summary>
    /// Method should return bad request result when prefix is empty.
    /// </summary>
    [Test]
    public async Task GetStationsByPrefix_WithEmptyPrefix_ReturnsBadRequestResult()
    {
        // Arrange
        const string prefix = "";

        // Act
        var result = await _controller.GetStationsByPrefix(prefix);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    /// <summary>
    /// Method should return bad request result when prefix is null.
    /// </summary>
    [Test]
    public async Task GetStationsByPrefix_WithNullPrefix_ReturnsBadRequestResult()
    {
        // Arrange
        string? prefix = null;

        // Act
        var result = await _controller.GetStationsByPrefix(prefix);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    /// <summary>
    /// Method should return bad request result when prefix is invalid.
    /// </summary>
    [Test]
    public async Task GetStationsByPrefix_WithInvalidPrefix_ReturnsNotFoundResult()
    {
        // Arrange
        const string prefix = "invalid";
        _mockStationService.Setup(x => x.GetStationsByPrefix(prefix)).ReturnsAsync((SearchResponse)null);

        // Act
        var result = await _controller.GetStationsByPrefix(prefix);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    /// <summary>
    /// Method should return InternalServerError result when error occurs.
    /// </summary>
    [Test]
    public async Task GetStationsByPrefix_ThrowsException_ReturnsInternalServerErrorResult()
    {
        // Arrange
        const string prefix = "exception";
        _mockStationService.Setup(x => x.GetStationsByPrefix(It.IsAny<string>())).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetStationsByPrefix(prefix);

        // Assert
        var objectCodeResult = result.Result as ObjectResult;
        Assert.That(objectCodeResult?.StatusCode, Is.EqualTo(500));
    }
}
