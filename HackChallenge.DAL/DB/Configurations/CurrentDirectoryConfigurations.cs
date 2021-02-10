using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class CurrentDirectoryConfigurations : IEntityTypeConfiguration<CurrentDirectory>
    {
        public void Configure(EntityTypeBuilder<CurrentDirectory> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(f => f.LinuxSystem)
                   .WithOne(s => s.CurrentDirectory)
                   .HasForeignKey<LinuxSystem>(s => s.CurrentDirId);

            builder.HasMany(f => f.Files)
                   .WithOne(f => f.CurrentDirectory)
                   .HasForeignKey(d => d.CurrentDirectoryId);

            builder.HasMany(f => f.Directories)
                   .WithOne(d => d.CurrentDirectory)
                   .HasForeignKey(d => d.CurrentDirectoryId);
        }
    }
}
