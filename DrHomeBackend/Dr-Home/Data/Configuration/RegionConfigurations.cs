using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class RegionConfigurations : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Regions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();


            builder.HasOne(r => r._city).
                WithMany(c => c._regions)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
