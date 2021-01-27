using HackChallenge.DAL.Entities;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IUserAccessRepository : IRepository<User, int>
    {
        Task<User> GetUserByChatId(long chatId);

    }

}
