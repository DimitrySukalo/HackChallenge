using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class MainDirectoryRepository : IMainDirectoryRepository
    {
        ApplicationContext _db;

        public MainDirectoryRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<MainDirectory> GetByIdAsync(int id)
        {
            MainDirectory mainDirectory = await _db.MainDirectories.FirstOrDefaultAsync(d => d.Id == id);
            return mainDirectory;
        }
    }
}
