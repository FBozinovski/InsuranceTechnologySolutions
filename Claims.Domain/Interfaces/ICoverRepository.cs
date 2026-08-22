using Claims.Domain.Contexts;
using Claims.Domain.Models;
using Claims.Dto.Responses;

namespace Claims.Domain.Interfaces
{
    public interface ICoverRepository : IRepository<Cover, ClaimsContext>
    {
        Task<CoverResponse?> GetCoverResponseById(string id);
        Task<IEnumerable<CoverResponse>?> GetAllCoverResponses();
    }
}
