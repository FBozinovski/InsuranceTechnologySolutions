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
    public class CoverRepository : Repository<Cover, ClaimsContext>, ICoverRepository
    {
        public CoverRepository(ClaimsContext context) : base(context)
        {
        }


    }
}
