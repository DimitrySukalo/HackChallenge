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
        private FileRepository fileRepository;
        private WifiRepository wifiRepository;
        private CurrentDirectoryRepository currentDirectory;

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

        public IWifiRepository WifiRepository
        {
            get
            {
                if (wifiRepository == null)
                    wifiRepository = new WifiRepository(ApplicationContext);

                return wifiRepository;
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

        public async Task SaveAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }
    }
}
