using HackChallenge.DAL.Entities;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface ILinuxRepository : IIDRepository<LinuxSystem>
    {
        Task<LinuxSystem> GetByIP(string ip);
    }
}
