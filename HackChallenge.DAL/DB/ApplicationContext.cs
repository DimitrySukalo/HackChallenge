﻿using HackChallenge.DAL.DB.Configurations;
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
        public DbSet<PreviousDirectory> PreviousDirectories { get; set; }
        public DbSet<MainDirectory> MainDirectories { get; set; }

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
            modelBuilder.ApplyConfiguration(new MainDirectoryConfigurations());
            modelBuilder.ApplyConfiguration(new CurrentDirectoryConfigurations());
            modelBuilder.ApplyConfiguration(new PreviousDirectoryConfigurations());
            modelBuilder.ApplyConfiguration(new WifiConfigurations());
            modelBuilder.ApplyConfiguration(new WifiModuleConfigurations());
        }
    }
}
