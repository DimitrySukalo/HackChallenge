using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.LinuxSystem)
                   .WithOne(s => s.User)
                   .HasForeignKey<LinuxSystem>(s => s.Id);


            builder.HasOne(u => u.GlobalNetwork)
                   .WithMany(n => n.Users)
                   .HasForeignKey(u => u.GlobalNetworkId);
        }
    }
}
