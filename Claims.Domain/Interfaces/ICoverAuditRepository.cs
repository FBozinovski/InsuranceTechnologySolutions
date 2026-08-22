using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Claims.Domain.Contexts;
using Claims.Domain.Models;

namespace Claims.Domain.Interfaces
{
    public interface ICoverAuditRepository : IRepository<CoverAudit, AuditContext>
    {
    }
}
