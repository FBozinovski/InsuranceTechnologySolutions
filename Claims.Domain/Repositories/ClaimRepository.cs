using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;

namespace Claims.Domain.Repositories
{
    public class ClaimRepository : Repository<Claim, ClaimsContext>, IClaimRepository
    {
        public ClaimRepository(ClaimsContext context) : base(context)
        {
        }
    }
}
