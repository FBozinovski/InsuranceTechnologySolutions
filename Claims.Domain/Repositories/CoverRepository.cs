using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Claims.Domain.Contexts;
using Claims.Domain.Interfaces;
using Claims.Domain.Models;
using Claims.Dto.Responses;
using Microsoft.EntityFrameworkCore;

namespace Claims.Domain.Repositories
{
    public class CoverRepository : Repository<Cover, ClaimsContext>, ICoverRepository
    {
        public CoverRepository(ClaimsContext context) : base(context)
        {
        }

        public async Task<CoverResponse?> GetCoverResponseById(string id)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CoverResponse
                {
                    Id = c.Id,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Type = c.Type,
                    Premium = c.Premium
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CoverResponse>?> GetAllCoverResponses()
        {
            return await _dbSet
                .AsNoTracking()
                .Select(c => new CoverResponse
                {
                    Id = c.Id,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Type = c.Type,
                    Premium = c.Premium
                }).ToListAsync();
        }
    }
}
