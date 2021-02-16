using HackChallenge.DAL.DB;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserAccessRepository UserAccessRepository { get; }
        IDirectoryRepository DirectoryRepository { get; }
        IWifiModuleRepository WifiModuleRepository { get; }
        ILinuxRepository LinuxRepository { get; }
        IFileRepository FileRepository { get; }
        IWifiRepository WifiRepository { get; }
        ICurrentDirectoryRepository CurrentDirectoryRepository { get; }
        ApplicationContext ApplicationContext { get; }

        Task SaveAsync();
    }
}
