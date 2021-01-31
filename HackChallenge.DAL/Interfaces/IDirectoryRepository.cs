using HackChallenge.DAL.Entities;
using System.Collections.Generic;

namespace HackChallenge.DAL.Interfaces
{
    public interface IDirectoryRepository : IRepository<Directory, int>
    {
        Directory GetInDirectory(Directory directory);
    }
}
