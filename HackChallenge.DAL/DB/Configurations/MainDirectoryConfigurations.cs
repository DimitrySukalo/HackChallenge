using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class MainDirectoryConfigurations : IEntityTypeConfiguration<MainDirectory>
    {
        public void Configure(EntityTypeBuilder<MainDirectory> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.LinuxSystem)
                   .WithOne(s => s.MainDirectory)
                   .HasForeignKey<LinuxSystem>(s => s.MainDirectoryId);

            builder.HasMany(m => m.Files)
                   .WithOne(f => f.MainDirectory)
                   .HasForeignKey(d => d.MainDirectoryId);

            builder.HasMany(m => m.Directories)
                   .WithOne(d => d.MainDirectory)
                   .HasForeignKey(d => d.MainDirectoryId);
        }
    }
}
