using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackChallenge.DAL.DB
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=hackChallenge;Trusted_Connection=True;");
        }
    }
}
