using HackChallenge.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IDirectoryRepository : IRepository<Directory, int>
    {
        Directory GetInDirectory(Directory directory);
        IEnumerable<Directory> GetDirectoriesOfCurrentDirectory(int id);
        IEnumerable<Directory> GetDirsByMainDirId(int id);
        IEnumerable<Directory> GetDirsByPrevDirId(int id);
        Task<IEnumerable<Directory>> GetDirsOfLinuxSystemId(int id);
    }
}
