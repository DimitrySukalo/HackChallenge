using HackChallenge.DAL.DB;
using HackChallenge.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace HackChallenge.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private UserRepository userRepository;
        private DirectoryRepository directoryRepository;
        private WifiModuleRepository wifiModuleRepository;
        private LinuxRepository linuxRepository;
        private CurrentDirectoryRepository currentDirectory;
        private PreviousDirectoryRepository previousDirectory;
        private FileRepository fileRepository;
        private MainDirectoryRepository mainDirectory;
        private WifiRepository wifiRepository;

        public ApplicationContext ApplicationContext { get; }

        public UnitOfWork(ApplicationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), " was null.");

            ApplicationContext = context;
        }

        public IUserAccessRepository UserAccessRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(ApplicationContext);

                return userRepository;
            }
        }

        public IDirectoryRepository DirectoryRepository
        {
            get
            {
                if (directoryRepository == null)
                    directoryRepository = new DirectoryRepository(ApplicationContext);

                return directoryRepository;
            }
        }

        public IWifiModuleRepository WifiModuleRepository
        {
            get
            {
                if (wifiModuleRepository == null)
                    wifiModuleRepository = new WifiModuleRepository(ApplicationContext);

                return wifiModuleRepository;
            }
        }

        public ILinuxRepository LinuxRepository
        {
            get
            {
                if (linuxRepository == null)
                    linuxRepository = new LinuxRepository(ApplicationContext);

                return linuxRepository;
            }
        }

        public IFileRepository FileRepository
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new FileRepository(ApplicationContext);

                return fileRepository;
            }
        }

        public ICurrentDirectoryRepository CurrentDirectoryRepository
        {
            get
            {
                if (currentDirectory == null)
                    currentDirectory = new CurrentDirectoryRepository(ApplicationContext);

                return currentDirectory;
            }
        }

        public IPreviousDirectoryRepository PreviousDirectoryRepository
        {
            get
            {
                if (previousDirectory == null)
                    previousDirectory = new PreviousDirectoryRepository(ApplicationContext);

                return previousDirectory;
            }
        }

        public IMainDirectoryRepository MainDirectoryRepository
        {
            get
            {
                if (mainDirectory == null)
                    mainDirectory = new MainDirectoryRepository(ApplicationContext);

                return mainDirectory;
            }
        }

        public IWifiRepository WifiRepository
        {
            get
            {
                if (wifiRepository == null)
                    wifiRepository = new WifiRepository(ApplicationContext);

                return wifiRepository;
            }
        }

        public async Task SaveAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }
    }
}
