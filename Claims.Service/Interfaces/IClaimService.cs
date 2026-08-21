using Claims.Domain.Models;

namespace Claims.Service.Interfaces
{
    public interface IClaimService
    {
        Task AuditClaim(string id, string httpRequestType);
        Task<Claim> CreateAsync(Claim claim, string httpRequestType);
    }
}
