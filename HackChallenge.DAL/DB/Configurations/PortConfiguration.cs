using HackChallenge.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackChallenge.DAL.DB.Configurations
{
    public class PortConfiguration : IEntityTypeConfiguration<Port>
    {
        public void Configure(EntityTypeBuilder<Port> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Vulnerabilities)
                   .WithMany(v => v.Ports);
        }
    }
}
