using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class WifiConfigurations : IEntityTypeConfiguration<Wifi>
    {
        public void Configure(EntityTypeBuilder<Wifi> builder)
        {
            builder.HasKey(w => w.Id);

            builder.HasMany(w => w.WifiModules)
                   .WithMany(m => m.Wifis);

            builder.HasOne(w => w.GlobalNetwork)
                   .WithOne(n => n.Wifi)
                   .HasForeignKey<Wifi>(n => n.GlobalNetworkId);
        }
    }
}
