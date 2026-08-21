using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;

namespace Claims.Domain.Repositories
{
    public class ClaimAuditRepository : Repository<ClaimAudit, AuditContext>, IClaimAuditRepository
    {
        public ClaimAuditRepository(AuditContext context) : base(context)
        {
        }
    }
}
