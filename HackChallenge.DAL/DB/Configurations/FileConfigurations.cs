using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class FileConfigurations : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.CurrentDirectory)
                   .WithMany(c => c.Files)
                   .HasForeignKey(f => f.CurrentDirectoryId);

            builder.HasOne(f => f.PreviousDirectory)
                   .WithMany(p => p.Files)
                   .HasForeignKey(f => f.PreviousDirectoryId);

            builder.HasOne(f => f.MainDirectory)
                   .WithMany(m => m.Files)
                   .HasForeignKey(f => f.MainDirectoryId);

            builder.HasOne(f => f.Directory)
                   .WithMany(d => d.Files)
                   .HasForeignKey(f => f.DirectoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
