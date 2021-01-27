using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        Task<T> AddAsync(T entity);

        T Remove(T entity);

        Task<T> GetByIdAsync(K id);

        Task<T> RemoveByIdAsync(K id);

        IEnumerable<T> GetAll();

        Task SaveChangesAsync();
    }
}
