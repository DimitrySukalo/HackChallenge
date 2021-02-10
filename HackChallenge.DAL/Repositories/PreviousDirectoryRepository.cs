using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class PreviousDirectoryRepository : IPreviousDirectoryRepository
    {
        ApplicationContext _db;

        public PreviousDirectoryRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<PreviousDirectory> GetByIdAsync(int id)
        {
            PreviousDirectory previousDirectory = await _db.PreviousDirectories.FirstOrDefaultAsync(d => d.Id == id);
            return previousDirectory;
        }
    }
}
