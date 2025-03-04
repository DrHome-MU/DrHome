using Dr_Home.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Dr_Home.Data.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            //Reviews With Patients 
            builder.HasOne(r=>r.patient)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PatientId)
              .OnDelete(DeleteBehavior.NoAction);


            //Review With Doctor 

            builder.HasOne(r=>r.doctor)
                .WithMany(d=>d.Reviews)
                .HasForeignKey(r=>r.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
