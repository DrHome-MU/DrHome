using Dr_Home.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

           builder.HasOne(d => d._specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId);
        }
    }
}
