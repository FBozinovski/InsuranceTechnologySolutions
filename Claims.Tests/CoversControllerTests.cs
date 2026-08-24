using Claims.Controllers;
using Claims.Dto.Enumerations;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Claims.Tests
{
    public class CoversControllerTests
    {
        private readonly Mock<ICoverService> _coverServiceMock = new();

        private CoversController CreateController(string httpMethod = "GET")
        {
            return new CoversController(NullLogger<CoversController>.Instance, _coverServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { Request = { Method = httpMethod } }
                }
            };
        }

        [Fact]
        public async Task ComputePremiumAsync_ReturnsOkWithPremium_WhenServiceSucceeds()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(60);
            _coverServiceMock
                .Setup(s => s.ComputePremium(startDate, endDate, Enumerations.CoverType.Yacht))
                .Returns(1000m);
            var controller = CreateController();

            var result = await controller.ComputePremiumAsync(startDate, endDate, Enumerations.CoverType.Yacht);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1000m, okResult.Value);
        }

        [Fact]
        public async Task ComputePremiumAsync_Returns500_WhenServiceThrows()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(60);
            _coverServiceMock
                .Setup(s => s.ComputePremium(startDate, endDate, It.IsAny<Enumerations.CoverType>()))
                .Throws(new Exception("boom"));
            var controller = CreateController();

            var result = await controller.ComputePremiumAsync(startDate, endDate, Enumerations.CoverType.Yacht);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetAsync_ReturnsOkWithCovers_WhenServiceSucceeds()
        {
            var covers = new List<CoverResponse> { new() { Id = "1", Type = Enumerations.CoverType.Yacht } };
            _coverServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(covers);
            var controller = CreateController();

            var result = await controller.GetAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(covers, okResult.Value);
        }

        [Fact]
        public async Task GetAsync_Returns500_WhenServiceThrows()
        {
            _coverServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("boom"));
            var controller = CreateController();

            var result = await controller.GetAsync();

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task CreateAsync_ReturnsOkWithResponse_WhenServiceSucceeds()
        {
            var request = new CoverRequest { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(30), Type = Enumerations.CoverType.Yacht };
            var response = new CoverResponse { Id = "cover-1", Type = request.Type };
            _coverServiceMock
                .Setup(s => s.CreateAsync(request, "POST"))
                .ReturnsAsync(response);
            var controller = CreateController("POST");

            var result = await controller.CreateAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(response, okResult.Value);
            _coverServiceMock.Verify(s => s.CreateAsync(request, "POST"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new CoverRequest { StartDate = DateTime.Today.AddDays(-1) };
            _coverServiceMock
                .Setup(s => s.CreateAsync(request, It.IsAny<string>()))
                .ThrowsAsync(new ValidationException("StartDate cannot be in the past."));
            var controller = CreateController("POST");

            var result = await controller.CreateAsync(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("StartDate cannot be in the past.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAsync_Returns500_WhenServiceThrowsUnexpectedException()
        {
            var request = new CoverRequest();
            _coverServiceMock
                .Setup(s => s.CreateAsync(request, It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("boom"));
            var controller = CreateController("POST");

            var result = await controller.CreateAsync(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenServiceSucceeds()
        {
            _coverServiceMock.Setup(s => s.DeleteByIdAsync("1", "DELETE")).Returns(Task.CompletedTask);
            var controller = CreateController("DELETE");

            var result = await controller.DeleteAsync("1");

            Assert.IsType<NoContentResult>(result);
            _coverServiceMock.Verify(s => s.DeleteByIdAsync("1", "DELETE"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Returns500_WhenServiceThrows()
        {
            _coverServiceMock.Setup(s => s.DeleteByIdAsync("1", It.IsAny<string>())).ThrowsAsync(new Exception("boom"));
            var controller = CreateController("DELETE");

            var result = await controller.DeleteAsync("1");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkWithCover_WhenFound()
        {
            var response = new CoverResponse { Id = "1" };
            _coverServiceMock.Setup(s => s.GetByIdAsync("1")).ReturnsAsync(response);
            var controller = CreateController();

            var result = await controller.GetAsync("1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(response, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenCoverDoesNotExist()
        {
            _coverServiceMock.Setup(s => s.GetByIdAsync("missing")).ReturnsAsync((CoverResponse)null!);
            var controller = CreateController();

            var result = await controller.GetAsync("missing");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByIdAsync_Returns500_WhenServiceThrows()
        {
            _coverServiceMock.Setup(s => s.GetByIdAsync("1")).ThrowsAsync(new Exception("boom"));
            var controller = CreateController();

            var result = await controller.GetAsync("1");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }
    }
}
