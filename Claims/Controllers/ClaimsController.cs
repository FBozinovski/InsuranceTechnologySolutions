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
        private readonly Auditer _auditer;
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimService _claimService;

        public ClaimsController(ILogger<ClaimsController> logger, AuditContext auditContext, IClaimRepository claimRepository, IClaimService claimService)
        {
            _logger = logger;
            _claimRepository = claimRepository;
            _claimService = claimService;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet]
        public async Task<IEnumerable<Claim>> GetAsync()
        {
            return await _claimRepository.GetAllAsync();
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
            _auditer.AuditClaim(id, Request.Method);
            await _claimRepository.DeleteByIdAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<Claim> GetAsync(string id)
        {
            return await _claimRepository.GetByIdAsync(id);
        }
    }
}
