using Claims.Domain.Contexts;
using Claims.Domain.Models;

namespace Claims.Domain.Interfaces
{
    public interface ICoverRepository : IRepository<Cover, ClaimsContext>
    {
    }
}
