using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class CurrentDirectoryRepository : ICurrentDirectoryRepository
    {
        ApplicationContext _db;

        public CurrentDirectoryRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<CurrentDirectory> GetByIdAsync(int id)
        {
            CurrentDirectory currentDirectory = await _db.CurrentDirectories.FirstOrDefaultAsync(d => d.Id == id);
            return currentDirectory;
        }
    }
}
