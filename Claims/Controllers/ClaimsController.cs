
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
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var response = await _claimService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ClaimRequest request)
        {
            var response = await _claimService.CreateAsync(request, Request.Method);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _claimService.DeleteByIdAsync(id, Request.Method);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var response = await _claimService.GetByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }
    }
}
