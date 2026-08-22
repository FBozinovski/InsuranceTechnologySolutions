using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Responses;
using Microsoft.EntityFrameworkCore;

namespace Claims.Domain.Repositories
{
    public class ClaimRepository : Repository<Claim, ClaimsContext>, IClaimRepository
    {
        public ClaimRepository(ClaimsContext context) : base(context)
        {
        }

        public async Task<ClaimResponse?> GetClaimResponseById(string id)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ClaimResponse
                {
                    Id = c.Id,
                    CoverId = c.CoverId,
                    Created = c.Created,
                    Name = c.Name,
                    Type = c.Type,
                    DamageCost = c.DamageCost
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClaimResponse>?> GetAllClaimResponses()
        {
            return await _dbSet
                .AsNoTracking()
                .Select(c => new ClaimResponse
                {
                    Id = c.Id,
                    CoverId = c.CoverId,
                    Created = c.Created,
                    Name = c.Name,
                    Type = c.Type,
                    DamageCost = c.DamageCost
                }).ToListAsync();
        }
    }
}
