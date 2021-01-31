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

        public async Task SaveAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }
    }
}
