using HackChallenge.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IUserAccessRepository : IRepository<User, int>
    {
        Task<User> GetUserByChatId(long chatId);
        Task AddRange(List<User> users);
        Task<List<User>> GetUsersByGlobalNetworkId(int id);
    }

}
