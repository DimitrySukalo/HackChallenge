using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IIDRepository<T> where T: class
    {
        Task<T> GetByIdAsync(int id);
    }
}
