using Claims.Domain.Models;
using Claims.Dto.Requests;
using Claims.Dto.Responses;

namespace Claims.Service.Interfaces
{
    public interface IClaimService
    {
        Task AuditClaim(string id, string httpRequestType);
        Task<ClaimResponse> CreateAsync(ClaimRequest claim, string httpRequestType);
        Task<IEnumerable<ClaimResponse>> GetAllAsync();
        Task DeleteByIdAsync(string id, string httpRequestType);
        Task<ClaimResponse> GetByIdAsync(string id);
    }
}
