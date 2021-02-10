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
        ICurrentDirectoryRepository CurrentDirectoryRepository { get; }
        IPreviousDirectoryRepository PreviousDirectoryRepository { get; }
        IMainDirectoryRepository MainDirectoryRepository { get; }
        IWifiRepository WifiRepository { get; }
        ApplicationContext ApplicationContext { get; }

        Task SaveAsync();
    }
}
