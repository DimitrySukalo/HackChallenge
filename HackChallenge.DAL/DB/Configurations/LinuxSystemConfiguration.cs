using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class LinuxSystemConfiguration : IEntityTypeConfiguration<LinuxSystem>
    {
        public void Configure(EntityTypeBuilder<LinuxSystem> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.User)
                   .WithOne(u => u.LinuxSystem)
                   .HasForeignKey<LinuxSystem>(s => s.Id);

            builder.HasOne(s => s.WifiModule)
                   .WithOne(m => m.LinuxSystem)
                   .HasForeignKey<LinuxSystem>(k => k.WifiModuleId);

            builder.HasOne(s => s.CurrentDirectory)
                   .WithOne(c => c.LinuxSystem)
                   .HasForeignKey<LinuxSystem>(s => s.CurrentDirectoryId);

            builder.HasMany(s => s.AllDirectories)
                   .WithOne(d => d.LinuxSystem);
        }
    }
}
