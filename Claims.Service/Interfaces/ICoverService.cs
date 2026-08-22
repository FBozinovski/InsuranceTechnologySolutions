using Claims.Dto.Enumerations;
using Claims.Dto.Requests;
using Claims.Dto.Responses;

namespace Claims.Service.Interfaces
{
    public interface ICoverService
    {
        Task<IEnumerable<CoverResponse>> GetAllAsync();
        decimal ComputePremium(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType);
        Task AuditCover(string id, string httpRequestType);
        Task<CoverResponse> CreateAsync(CoverRequest cover, string httpRequestType);
        Task<CoverResponse> GetByIdAsync(string id);
        Task DeleteByIdAsync(string id, string httpRequestType);
    }
}
