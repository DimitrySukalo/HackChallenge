using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackChallenge.DAL.DB
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<LinuxSystem> LinuxSystems { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Wifi> Wifis { get; set; }
        public virtual DbSet<Modem> Modems { get; set; }


        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=hackChallenge;Trusted_Connection=True;");
        }
    }
}
