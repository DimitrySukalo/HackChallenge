using HackChallenge.DAL.Entities;
using System.Collections.Generic;

namespace HackChallenge.DAL.Interfaces
{
    public interface IFileRepository
    {
        IEnumerable<File> GetFilesByCurrentDirId(int id);
        IEnumerable<File> GetFilesByMainDirId(int id);
        IEnumerable<File> GetFilesByPrevDirId(int id);
        IEnumerable<File> GetFilesOfDir(int id);
        Dictionary<int, List<File>> GetFilesOfSomeDirs(List<int> ids);
    }
}
