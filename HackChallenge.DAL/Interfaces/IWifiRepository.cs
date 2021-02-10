using HackChallenge.DAL.Entities;
using System.Collections.Generic;

namespace HackChallenge.DAL.Interfaces
{
    public interface IWifiRepository
    {
        IEnumerable<Wifi> GetByWifisModuleId(int id);
    }
}
