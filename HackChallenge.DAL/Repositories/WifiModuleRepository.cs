using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class WifiModuleRepository : IWifiModuleRepository
    {
        ApplicationContext _db;

        public WifiModuleRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        public async Task<WifiModule> AddAsync(WifiModule entity)
        {
            if (entity != null)
            {
                await _db.WifiModules.AddAsync(entity);
                await _db.SaveChangesAsync();

                return entity;
            }

            return null;
        }

        public IEnumerable<WifiModule> GetAll()
        {
            IEnumerable<WifiModule> wifiModules = _db.WifiModules;
            return wifiModules;
        }

        public async Task<WifiModule> GetByIdAsync(int id)
        {
            WifiModule wifiModule = await _db.WifiModules.FirstOrDefaultAsync(m => m.Id == id);
            return wifiModule;
        }

        public WifiModule Remove(WifiModule entity)
        {
            if (entity != null)
            {
                _db.WifiModules.Remove(entity);
                _db.SaveChanges();

                return entity;
            }

            return null;
        }

        public async Task<WifiModule> RemoveByIdAsync(int id)
        {
            WifiModule wifiModule = await _db.WifiModules.FirstOrDefaultAsync(u => u.Id == id);
            if (wifiModule != null)
            {
                _db.WifiModules.Remove(wifiModule);
                await _db.SaveChangesAsync();

                return wifiModule;
            }

            return null;
        }
    }
}
