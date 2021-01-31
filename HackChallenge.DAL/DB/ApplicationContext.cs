using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackChallenge.DAL.DB
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<LinuxSystem> LinuxSystems { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Wifi> Wifis { get; set; }
        public DbSet<WifiModule> WifiModules { get; set; }
        public DbSet<CurrentDirectory> CurrentDirectories { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
