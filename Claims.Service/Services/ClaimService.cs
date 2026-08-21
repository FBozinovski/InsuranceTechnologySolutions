
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Service.Interfaces;

namespace Claims.Service.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimAuditRepository _claimAuditRepository;

        public ClaimService(IClaimRepository claimRepository, IClaimAuditRepository claimAuditRepository)
        {
            _claimRepository = claimRepository;
            _claimAuditRepository = claimAuditRepository;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await _claimAuditRepository.AddAsync(claimAudit);
        }

        //TODO: create a request class and don't use the domain model directly in the service layer. This is a bad practice and should be avoided.
        public async Task<Claim> CreateAsync(Claim claim, string httpRequestType)
        {
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.AddAsync(claim);
            await AuditClaim(claim.Id, httpRequestType);

            return claim;
        }
    }
}
