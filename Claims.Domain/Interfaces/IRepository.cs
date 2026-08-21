using Microsoft.EntityFrameworkCore;

namespace Claims.Domain.Interfaces
{
    public interface IRepository<T, C> where T : class, new() where C : DbContext
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(object id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
