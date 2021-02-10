using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class PreviousDirectoryConfigurations : IEntityTypeConfiguration<PreviousDirectory>
    {
        public void Configure(EntityTypeBuilder<PreviousDirectory> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.LinuxSystem)
                   .WithOne(s => s.PreviousDirectory)
                   .HasForeignKey<LinuxSystem>(s => s.PreviousDirectoryId);

            builder.HasMany(p => p.Files)
                   .WithOne(f => f.PreviousDirectory)
                   .HasForeignKey(d => d.PreviousDirectoryId);

            builder.HasMany(p => p.Directories)
                   .WithOne(d => d.PreviousDirectory)
                   .HasForeignKey(d => d.PreviousDirectoryId);
        }
    }
}
