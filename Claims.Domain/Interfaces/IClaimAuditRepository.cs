using Claims.Domain.Contexts;
using Claims.Domain.Models;

namespace Claims.Domain.Interfaces
{
    public interface IClaimAuditRepository : IRepository<ClaimAudit, AuditContext> 
    {
    }
}
