using Claims.Domain.Contexts;
using Claims.Domain.Models;
using Claims.Dto.Responses;

namespace Claims.Domain.Interfaces
{
    public interface IClaimRepository : IRepository<Claim, ClaimsContext>
    {
        Task<ClaimResponse?> GetClaimResponseById(string id);
        Task<IEnumerable<ClaimResponse>?> GetAllClaimResponses();
    }
}
