
using Claims.Dto.Requests;
using Claims.Dto.Responses;
using Claims.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService, ILogger<ClaimsController> logger)
        {
            _claimService = claimService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            IEnumerable<ClaimResponse> response;
            try
            {
                response = await _claimService.GetAllAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ClaimRequest request)
        {
            ClaimResponse response;
            try
            {
                response = await _claimService.CreateAsync(request, Request.Method);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await _claimService.DeleteByIdAsync(id, Request.Method);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            ClaimResponse response;
            try
            { 
                response = await _claimService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return response is not null ? Ok(response) : NotFound();
        }
    }
}
