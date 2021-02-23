using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class LinuxRepository : ILinuxRepository
    {
        ApplicationContext _db;

        public LinuxRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<LinuxSystem> GetByIdAsync(int id)
        {
            LinuxSystem linuxSystem = await _db.LinuxSystems.FirstOrDefaultAsync(s => s.Id == id);
            return linuxSystem;
        }

        public async Task<LinuxSystem> GetByIP(string ip)
        {
            LinuxSystem linuxSystem = await _db.LinuxSystems.FirstOrDefaultAsync(s => s.IP == ip);
            return linuxSystem;
        }
    }
}
