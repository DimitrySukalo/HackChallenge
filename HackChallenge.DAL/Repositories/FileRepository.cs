using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        ApplicationContext _db;

        public FileRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task AddAsync(File file)
        {
            if(file != null)
            {
                await _db.Files.AddAsync(file);
                await _db.SaveChangesAsync();
            }
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
