using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class DirectoryConfigurations : IEntityTypeConfiguration<Directory>
    {
        public void Configure(EntityTypeBuilder<Directory> builder)
        {
            builder.HasKey(d => d.Id);


            builder.HasOne(d => d.CurrentDirectory)
                   .WithMany(c => c.Directories)
                   .HasForeignKey(d => d.CurrentDirectoryId);

            builder.HasOne(d => d.PreviousDirectory)
                   .WithMany(p => p.Directories)
                   .HasForeignKey(d => d.PreviousDirectoryId);

            builder.HasOne(d => d.MainDirectory)
                   .WithMany(m => m.Directories)
                   .HasForeignKey(d => d.MainDirectoryId);

            builder.HasOne(d => d.LinuxSystem)
                   .WithMany(s => s.AllDirectories)
                   .HasForeignKey(d => d.LinuxSystemId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
