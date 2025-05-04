using Dr_Home.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.ToTable("Clinics");

            //  builder.HasIndex(x => new { x.ClinicName, x.city, x.region }).IsUnique();

            builder.Property(c => c.DetailedAddress)
                .HasMaxLength(100);

            //One-To-Many (doctor & clinics)
            builder.HasOne(c => c.doctor)
                .WithMany(d => d.clinics)
                .HasForeignKey(c => c.DoctorId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
