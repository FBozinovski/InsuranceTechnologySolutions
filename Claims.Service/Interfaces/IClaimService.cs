using Claims.Domain.Models;

namespace Claims.Service.Interfaces
{
    public interface IClaimService
    {
        Task AuditClaim(string id, string httpRequestType);
        Task<Claim> CreateAsync(Claim claim, string httpRequestType);
        Task<IEnumerable<Claim>> GetAllAsync();
        Task DeleteByIdAsync(string id, string httpRequestType);
        Task<Claim> GetByIdAsync(string id);
    }
}
