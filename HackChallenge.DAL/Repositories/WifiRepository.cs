using HackChallenge.DAL.DB;
using HackChallenge.DAL.Entities;
using HackChallenge.DAL.Interfaces;
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
            IEnumerable<Wifi> wifis = _db.Wifis.Where(w => w.WifiModuleId == id);
            return wifis;
        }
    }
}
