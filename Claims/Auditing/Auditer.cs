using Claims.Domain.Contexts;
using Claims.Domain.Models;

namespace Claims.Auditing
{
    public class Auditer
    {
        private readonly AuditContext _auditContext;

        public Auditer(AuditContext auditContext)
        {
            _auditContext = auditContext;
        }

      
        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditContext.Add(coverAudit);
            _auditContext.SaveChanges();
        }
    }
}
