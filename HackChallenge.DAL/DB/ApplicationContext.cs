using HackChallenge.DAL.DB.Configurations;
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
        public DbSet<GlobalNetwork> GlobalNetworks { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        public DbSet<Port> Ports { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LinuxSystemConfiguration());
            modelBuilder.ApplyConfiguration(new DirectoryConfigurations());
            modelBuilder.ApplyConfiguration(new FileConfigurations());
            modelBuilder.ApplyConfiguration(new WifiConfigurations());
            modelBuilder.ApplyConfiguration(new CurrentDirectoryConfiguration());
            modelBuilder.ApplyConfiguration(new WifiModuleConfigurations());
            modelBuilder.ApplyConfiguration(new GlobalNetworkConfiguration());
            modelBuilder.ApplyConfiguration(new PortConfiguration());
            modelBuilder.ApplyConfiguration(new VulnerabilityConfiguration());
        }
    }
}
