using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HackChallenge.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        ApplicationContext _db;

        public FileRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public IEnumerable<File> GetFilesByCurrentDirId(int id)
        {
            IEnumerable<File> files = _db.Files.Where(f => f.CurrentDirectoryId == id);
            return files;
        }

        public IEnumerable<File> GetFilesByMainDirId(int id)
        {
            IEnumerable<File> files = _db.Files.Where(f => f.MainDirectoryId == id);
            return files;
        }

        public IEnumerable<File> GetFilesByPrevDirId(int id)
        {
            IEnumerable<File> files = _db.Files.Where(f => f.PreviousDirectoryId == id);
            return files;
        }

        public IEnumerable<File> GetFilesOfDir(int id)
        {
            IEnumerable<File> files = _db.Files.Where(f => f.DirectoryId == id);
            return files;
        }

        public Dictionary<int, List<File>> GetFilesOfSomeDirs(List<int> ids)
        {
            Dictionary<int, List<File>> files = new Dictionary<int, List<File>>();
            foreach(int id in ids)
            {
                List<File> tempFiles = _db.Files.Where(f => f.DirectoryId == id).ToList();
                files.Add(id, tempFiles);
            }

            return files;
        }
    }
}
