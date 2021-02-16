using HackChallenge.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IFileRepository
    {
        IEnumerable<File> GetFilesOfDir(int id);
        Dictionary<int, List<File>> GetFilesOfSomeDirs(List<int> ids);

        Task AddAsync(File file);
        Task<File> GetByPath(string path);
    }
}
