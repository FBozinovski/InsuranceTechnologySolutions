using Claims.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claims.Service.Interfaces
{
    public interface ICoverService
    {
        Task<IEnumerable<Cover>> GetAllAsync();
        decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
        Task AuditCover(string id, string httpRequestType);
        Task<Cover> CreateAsync(Cover cover, string httpRequestType);
        Task<Cover> GetByIdAsync(string id);
        Task DeleteByIdAsync(string id, string httpRequestType);
    }
}
