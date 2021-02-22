using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class GlobalNetworkConfiguration : IEntityTypeConfiguration<GlobalNetwork>
    {
        public void Configure(EntityTypeBuilder<GlobalNetwork> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.Wifi)
                   .WithOne(w => w.GlobalNetwork)
                   .HasForeignKey<Wifi>(n => n.GlobalNetworkId);

            builder.HasMany(n => n.Users)
                   .WithOne(u => u.GlobalNetwork)
                   .HasForeignKey(u => u.GlobalNetworkId);
        }
    }
}
