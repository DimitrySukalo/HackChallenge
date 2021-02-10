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

            builder.HasOne(w => w.WifiModule)
                   .WithMany(m => m.Wifis)
                   .HasForeignKey(w => w.WifiModuleId);
        }
    }
}
