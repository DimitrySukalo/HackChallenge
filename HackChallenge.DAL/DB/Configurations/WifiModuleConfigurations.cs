using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class WifiModuleConfigurations : IEntityTypeConfiguration<WifiModule>
    {
        public void Configure(EntityTypeBuilder<WifiModule> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(w => w.LinuxSystem)
                   .WithOne(s => s.WifiModule)
                   .HasForeignKey<LinuxSystem>(s => s.WifiModuleId);

            builder.HasMany(m => m.Wifis)
                   .WithOne(m => m.WifiModule)
                   .HasForeignKey(w => w.WifiModuleId);
        }
    }
}
