using Claims.Domain.Models;
using Claims.Dto.Enumerations;

namespace Claims.Service.Interfaces
{
    public interface ICoverService
    {
        Task<IEnumerable<Cover>> GetAllAsync();
        decimal ComputePremium(DateTime startDate, DateTime endDate, Enumerations.CoverType coverType);
        Task AuditCover(string id, string httpRequestType);
        Task<Cover> CreateAsync(Cover cover, string httpRequestType);
        Task<Cover> GetByIdAsync(string id);
        Task DeleteByIdAsync(string id, string httpRequestType);
    }
}
