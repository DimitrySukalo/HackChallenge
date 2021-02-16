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

        public CurrentDirectoryRepository(ApplicationContext applicationContext)
        {
            _db = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext), " was null.");
        }

        public async Task<CurrentDirectory> GetByIdAsync(int id)
        {
            return await _db.CurrentDirectories.FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
