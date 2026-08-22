using Claims.Auditing;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
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

        public ClaimsController(ILogger<ClaimsController> logger, IClaimService claimService)
        {
            _logger = logger;
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IEnumerable<Claim>> GetAsync()
        {
            return await _claimService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim request)
        {
            var claim = await _claimService.CreateAsync(request, Request.Method);
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _claimService.DeleteByIdAsync(id, Request.Method);
        }

        [HttpGet("{id}")]
        public async Task<Claim> GetAsync(string id)
        {
            return await _claimService.GetByIdAsync(id);
        }
    }
}
