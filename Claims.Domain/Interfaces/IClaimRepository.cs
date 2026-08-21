using Claims.Domain.Contexts;
using Claims.Domain.Models;

namespace Claims.Domain.Interfaces
{
    public interface IClaimRepository : IRepository<Claim, ClaimsContext>
    {
    }
}
