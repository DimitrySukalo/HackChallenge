using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HackChallenge.DAL.Repositories
{
    public class WifiRepository : IWifiRepository
    {
        ApplicationContext _db;

        public WifiRepository(ApplicationContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context), " was null.");
        }

        /// <summary>
        /// Get wifis by wifi module id
        /// </summary>
        /// <param name="id">Wifi module id</param>
        /// <returns></returns>
        public IEnumerable<Wifi> GetByWifisModuleId(int id)
        {
            IEnumerable<WifiModule> modules = _db.WifiModules.Include(w => w.Wifis).Where(m => m.Id == id);
            List<Wifi> wifis = new List<Wifi>();

            foreach(var module in modules)
            {
                wifis.AddRange(module.Wifis);
            }

            return wifis;
        }
    }
}
