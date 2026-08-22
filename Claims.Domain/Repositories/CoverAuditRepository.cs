using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;

namespace Claims.Domain.Repositories
{
    public class CoverAuditRepository : Repository<CoverAudit, AuditContext>, ICoverAuditRepository
    {
        public CoverAuditRepository(AuditContext context) : base(context)
        {
        }
    }
}
