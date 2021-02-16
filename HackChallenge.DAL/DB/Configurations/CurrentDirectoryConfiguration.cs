using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class CurrentDirectoryConfiguration : IEntityTypeConfiguration<CurrentDirectory>
    {
        public void Configure(EntityTypeBuilder<CurrentDirectory> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Directory)
                   .WithOne(d => d.CurrentDirectory)
                   .HasForeignKey<CurrentDirectory>(d => d.DirectoryId);

            builder.HasOne(d => d.LinuxSystem)
                   .WithOne(s => s.CurrentDirectory)
                   .HasForeignKey<LinuxSystem>(s => s.CurrentDirectoryId);
        }
    }
}
