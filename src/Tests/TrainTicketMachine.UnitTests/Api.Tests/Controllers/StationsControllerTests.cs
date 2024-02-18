using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTicketMachine.Api.Controllers;
using TrainTicketMachine.Core.Interfaces;
using TrainTicketMachine.Infrastructure.Models;

namespace TrainTicketMachine.UnitTests.Api.Tests.Controllers
{
    public class StationsControllerTests
    {
        private Mock<IStationService> _mockStationService;
        private StationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockStationService = new Mock<IStationService>();
            _controller = new StationsController(_mockStationService.Object);
        }

        /// <summary>
        /// Method should return ok result.
        /// </summary>
        [Test]
        public async Task GetStationsByPrefix_WithValidPrefix_ReturnsOkResult()
        {
            // Arrange
            var prefix = "abc";
            var expectedResponse = new SearchResponse
            {
                NextLetters = new System.Collections.Generic.List<char> { 'd', 'e', 'f' },
                StationsNames = new System.Collections.Generic.List<string> { "Station1", "Station2" }
            };
            _mockStationService.Setup(x => x.GetStationsByPrefix(prefix)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetStationsByPrefix(prefix);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult.Value);
        }

        /// <summary>
        /// Method should return bad request result when prefix is empty.
        /// </summary>
        [Test]
        public async Task GetStationsByPrefix_WithEmptyPrefix_ReturnsBadRequestResult()
        {
            // Arrange
            string prefix = "";

            // Act
            var result = await _controller.GetStationsByPrefix(prefix);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Method should return bad request result when prefix is null.
        /// </summary>
        [Test]
        public async Task GetStationsByPrefix_WithNullPrefix_ReturnsBadRequestResult()
        {
            // Arrange
            string prefix = null;

            // Act
            var result = await _controller.GetStationsByPrefix(prefix);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Method should return bad request result when prefix is invalid.
        /// </summary>
        [Test]
        public async Task GetStationsByPrefix_WithInvalidPrefix_ReturnsNotFoundResult()
        {
            // Arrange
            var prefix = "invalid";
            _mockStationService.Setup(x => x.GetStationsByPrefix(prefix)).ReturnsAsync((SearchResponse)null);

            // Act
            var result = await _controller.GetStationsByPrefix(prefix);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        /// <summary>
        /// Method should return InternalServerError result when error occurs.
        /// </summary>
        [Test]
        public async Task GetStationsByPrefix_ThrowsException_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var prefix = "exception";
            _mockStationService.Setup(x => x.GetStationsByPrefix(It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetStationsByPrefix(prefix);

            // Assert
            var objectCodeResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectCodeResult.StatusCode);
        }
    }
}
