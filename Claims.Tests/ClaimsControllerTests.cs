using Claims.Controllers;
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        private readonly Mock<IClaimService> _claimServiceMock = new();

        private ClaimsController CreateController(string httpMethod = "GET")
        {
            return new ClaimsController(_claimServiceMock.Object, NullLogger<ClaimsController>.Instance)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { Request = { Method = httpMethod } }
                }
            };
        }

        [Fact]
        public async Task Get_Claims()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                    { });

            var client = application.CreateClient();

            var response = await client.GetAsync("/Claims");

            response.EnsureSuccessStatusCode();

            //TODO: Apart from ensuring 200 OK being returned, what else can be asserted?
        }

        [Fact]
        public async Task GetAsync_ReturnsOkWithClaims_WhenServiceSucceeds()
        {
            var claims = new List<ClaimResponse> { new() { Id = "1", Name = "Broken mast" } };
            _claimServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(claims);
            var controller = CreateController();

            var result = await controller.GetAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(claims, okResult.Value);
        }

        [Fact]
        public async Task GetAsync_Returns500_WhenServiceThrows()
        {
            _claimServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("boom"));
            var controller = CreateController();

            var result = await controller.GetAsync();

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task CreateAsync_ReturnsOkWithResponse_WhenServiceSucceeds()
        {
            var request = new ClaimRequest { CoverId = "cover-1", Name = "Broken mast", DamageCost = 500m, Created = DateTime.Today };
            var response = new ClaimResponse { Id = "claim-1", Name = request.Name };
            _claimServiceMock
                .Setup(s => s.CreateAsync(request, "POST"))
                .ReturnsAsync(response);
            var controller = CreateController("POST");

            var result = await controller.CreateAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(response, okResult.Value);
            _claimServiceMock.Verify(s => s.CreateAsync(request, "POST"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new ClaimRequest { DamageCost = 1_000_000m };
            _claimServiceMock
                .Setup(s => s.CreateAsync(request, It.IsAny<string>()))
                .ThrowsAsync(new ValidationException("DamageCost cannot exceed 100,000."));
            var controller = CreateController("POST");

            var result = await controller.CreateAsync(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("DamageCost cannot exceed 100,000.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAsync_Returns500_WhenServiceThrowsUnexpectedException()
        {
            var request = new ClaimRequest();
            _claimServiceMock
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
            _claimServiceMock.Setup(s => s.DeleteByIdAsync("1", "DELETE")).Returns(Task.CompletedTask);
            var controller = CreateController("DELETE");

            var result = await controller.DeleteAsync("1");

            Assert.IsType<NoContentResult>(result);
            _claimServiceMock.Verify(s => s.DeleteByIdAsync("1", "DELETE"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Returns500_WhenServiceThrows()
        {
            _claimServiceMock.Setup(s => s.DeleteByIdAsync("1", It.IsAny<string>())).ThrowsAsync(new Exception("boom"));
            var controller = CreateController("DELETE");

            var result = await controller.DeleteAsync("1");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkWithClaim_WhenFound()
        {
            var response = new ClaimResponse { Id = "1" };
            _claimServiceMock.Setup(s => s.GetByIdAsync("1")).ReturnsAsync(response);
            var controller = CreateController();

            var result = await controller.GetAsync("1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Same(response, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenClaimDoesNotExist()
        {
            _claimServiceMock.Setup(s => s.GetByIdAsync("missing")).ReturnsAsync((ClaimResponse)null!);
            var controller = CreateController();

            var result = await controller.GetAsync("missing");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByIdAsync_Returns500_WhenServiceThrows()
        {
            _claimServiceMock.Setup(s => s.GetByIdAsync("1")).ThrowsAsync(new Exception("boom"));
            var controller = CreateController();

            var result = await controller.GetAsync("1");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }
    }
}
