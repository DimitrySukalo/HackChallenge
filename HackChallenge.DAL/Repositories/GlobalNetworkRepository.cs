using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class GlobalNetworkRepository : IGlobalNetworkRepository
    {
        ApplicationContext _db;

        public GlobalNetworkRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<GlobalNetwork> GetByIdAsync(int id)
        {
            return await _db.GlobalNetworks.FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}
