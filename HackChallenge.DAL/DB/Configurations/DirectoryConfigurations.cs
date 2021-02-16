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
                   .WithOne(c => c.Directory)
                   .HasForeignKey<CurrentDirectory>(c => c.DirectoryId);

            builder.HasOne(d => d.LinuxSystem)
                   .WithMany(s => s.AllDirectories)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
