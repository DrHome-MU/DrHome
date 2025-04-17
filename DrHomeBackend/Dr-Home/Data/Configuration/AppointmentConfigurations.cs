using Dr_Home.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            //Appointment & Schedule

            builder.HasOne(a=>a._schedule)
                .WithMany(s=>s._appointments)
                .HasForeignKey(a=>a.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            //Appointment & Patient
            builder.HasOne(a => a._patient)
               .WithMany(p => p._appointments)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Restrict);


            //Appointment & Doctor

            builder.HasOne(a => a._doctor)
               .WithMany(d => d._appointments)
               .HasForeignKey(a => a.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
